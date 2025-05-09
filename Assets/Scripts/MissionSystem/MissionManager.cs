using TMPro;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the mission queue and mission instances.
/// It also handles mission completion and failure.
/// And it provides optional UI elements for displaying mission info.
/// </summary>
public class MissionManager : MonoBehaviour
{
    #region Singleton

    public static MissionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Queue up initial missions.
        foreach (var mission in initialMissionQueue)
        {
            QueueMission(mission);
        }
    }

    #endregion

    #region Properties

    [Header("Mission Queue (Manage in Inspector)")]

    /// <summary>
    /// Missions that will be queued at startup.
    /// </summary>
    [Tooltip("Missions that will be queued at startup.")]
    public List<MissionData> initialMissionQueue = new List<MissionData>();

    /// <summary>
    /// The event relay for mission events.
    /// </summary>
    [Tooltip("The event relay for mission events.")]
    public MissionEventRelay eventRelay;

    [Header("Debug Info")]

    /// <summary>
    /// The current mission data.
    /// </summary>
    [Tooltip("The current mission data.")]
    public MissionData currentMissionData;

    /// <summary>
    /// The current mission instance.
    /// </summary>
    private Queue<MissionInstance> missionQueue = new Queue<MissionInstance>();

    /// <summary>
    /// The current mission instance.
    /// </summary>
    private MissionInstance currentMission;

    /// <summary>
    /// (Optional) UI element for displaying the mission name.
    /// </summary>
    private TMP_Text missionName;

    /// <summary>
    /// (Optional) UI element for displaying the mission description.
    /// </summary>
    private TMP_Text missionDescription;

    /// <summary>
    /// (Optional) UI element for displaying the mission timer.
    /// </summary>
    private TMP_Text missionTimer;

    #endregion

    private void Start()
    {
        // (Optional) Find UI elements for displaying mission info.
        if(missionName == null)
        {
            missionName = GameObject.Find("Mission_Name").GetComponent<TMP_Text>();
        }

        if(missionDescription == null)
        {
            missionDescription = GameObject.Find("Mission_Description").GetComponent<TMP_Text>();
        }

        if(missionTimer == null)
        {
            missionTimer = GameObject.Find("Mission_Timer").GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        // If no mission is in progress, start the next one.
        if (currentMission == null && missionQueue.Count > 0)
        {
            currentMission = missionQueue.Dequeue();
            StartMission(currentMission);
        }

        // If a mission is active, update its timer.
        if (currentMission != null && currentMission.status == MissionStatus.InProgress)
        {
            currentMission.elapsedTime = Time.time - currentMission.startTime;

            missionTimer.text = currentMission.missionData.isTimed ? "Time Limit: " + (currentMission.missionData.timeLimit - currentMission.elapsedTime).ToString("F1") + "s" : string.Empty;

            // If mission is timed and the time limit is exceeded, fail the mission.
            if (currentMission.missionData.isTimed && currentMission.elapsedTime > currentMission.missionData.timeLimit)
            {
                FailCurrentMission();
            }

            // Update debug info.
            currentMissionData = currentMission.missionData;
        }

        // Check if custom conditions are met automatically.
        if (currentMission != null && currentMission.status == MissionStatus.InProgress)
        {
            // Check if custom conditions are set and all are met.
            if (currentMission.missionData.customConditions != null && currentMission.missionData.customConditions.Count > 0)
            {
                bool allConditionsMet = true;
                foreach (var conditionObj in currentMission.missionData.customConditions)
                {
                    // Attempt to cast to IMissionCondition.
                    IMissionCondition condition = conditionObj as IMissionCondition;
                    if (condition != null && !condition.IsConditionMet())
                    {
                        allConditionsMet = false;
                        break;
                    }
                }
                if (allConditionsMet)
                {
                    Debug.Log("All mission conditions met automatically.");
                    CompleteCurrentMission();
                }
            }
        }
    }

    #region Uitility Methods

    /// <summary>
    /// Queues a new mission based on the MissionData asset.
    /// </summary>
    /// <param name="missionData"> The mission data to be queued. </param>
    public void QueueMission(MissionData missionData)
    {
        MissionInstance instance = new MissionInstance(missionData);
        missionQueue.Enqueue(instance);
    }

    /// <summary>
    /// Starts the given mission instance.
    /// </summary>
    /// <param name="mission"> An instance of the mission to be started. </param>
    private void StartMission(MissionInstance mission)
    {
        mission.status = MissionStatus.InProgress;
        mission.startTime = Time.time;
        mission.elapsedTime = 0f;
        Debug.Log("Mission started: " + mission.missionData.missionName);

        // (Optional) Update debug info for UI.
        missionName.text = mission.missionData.missionName;
        missionDescription.text = mission.missionData.missionDescription;
        missionTimer.text = mission.missionData.isTimed ? "Time Limit: " + mission.missionData.timeLimit + "s" : string.Empty;
    }

    #endregion

    #region Events

    /// <summary>
    /// Call this when the current mission is successfully completed.
    /// It invokes events via the relay, applies rewards, and queues any subtasks.
    /// </summary>
    public void CompleteCurrentMission()
    {
        if (currentMission != null)
        {
            if (currentMission != null)
            {
                currentMission.status = MissionStatus.Completed;
                Debug.Log("Mission completed: " + currentMission.missionData.missionName);

                // Invoke the runtime relay event.
                if (eventRelay != null)
                {
                    eventRelay.RelayComplete();
                }

                // Process and apply rewards.
                if (currentMission.missionData.rewards != null)
                {
                    foreach (var reward in currentMission.missionData.rewards)
                    {
                        reward.ApplyReward();
                    }
                }

                // Queue any sub-missions.
                if (currentMission.missionData.subMissions != null)
                {
                    foreach (var sub in currentMission.missionData.subMissions)
                    {
                        QueueMission(sub);
                    }
                }

                currentMission = null;
            }
        }
    }

    /// <summary>
    /// Call this when the current mission fails (e.g., due to time running out).
    /// Invokes the mission's onMissionFail event.
    /// </summary>
    public void FailCurrentMission()
    {
        if (currentMission != null)
        {
            currentMission.status = MissionStatus.Failed;
            Debug.Log("Mission failed: " + currentMission.missionData.missionName);
            if (eventRelay != null)
            {
                eventRelay.RelayFail();
            }
            currentMission = null;
        }
    }

    #endregion
}