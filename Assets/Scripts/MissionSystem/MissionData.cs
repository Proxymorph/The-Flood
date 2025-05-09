using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ScriptableObject that holds the data for a mission.
/// </summary>

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Mission Data")]
public class MissionData : ScriptableObject
{
    [Header("Basic Info")]

    /// <summary>
    /// The name of the mission.
    /// </summary>
    [Tooltip("The name of the mission.")]
    public string missionName;

    /// <summary>
    /// The description of the mission.
    /// </summary>
    [Tooltip("The description of the mission.")]
    [TextArea]
    public string missionDescription;

    [Header("Timing")]

    /// <summary>
    /// Is this mission timed?
    /// </summary>
    [Tooltip("Is this mission timed?")]
    public bool isTimed;

    /// <summary>
    /// Time limit in seconds (if timed).
    /// </summary>
    [Tooltip("Time limit in seconds (if timed).")]
    public float timeLimit;

    [Header("Submissions")]

    /// <summary>
    /// List of sub-missions that must be completed to finish this mission.
    /// </summary>
    [Tooltip("List of sub-missions that must be completed to finish this mission.")]
    public List<MissionData> subMissions;

    [Header("Rewards")]

    /// <summary>
    /// List of rewards granted on mission completion.
    /// </summary>
    [Tooltip("List of rewards granted on mission completion.")]
    public List<BaseReward> rewards;

    [Header("Custom Conditions")]

    /// <summary>
    /// List of custom conditions that must be met to complete the mission.
    /// </summary>
    [Tooltip("List of custom conditions that must be met to complete the mission.")]
    public List<ScriptableObject> customConditions;
}