using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AfflictionManager is a singleton class that manages the active afflictions on the player.
/// </summary>
public class AfflictionManager : MonoBehaviour
{
    #region Singleton

    // Singleton implementation
    public static AfflictionManager Instance { get; private set; }

    void Awake()
    {
        // Singleton implementation: destroy duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Make this persist across scenes:
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Affliction Types

    /// <summary>
    /// The list of active afflictions on the player.
    /// It is a list of <see cref="IAffliction"/> interfaces, so it can hold any type of affliction.
    /// </summary>
    private List<IAffliction> activeAfflictions = new List<IAffliction>();

    [Header("Affliction Data")]

    /// <summary>
    /// Data for the Drowsiness affliction. It is a <see cref="AfflictionData"/> that holds the affliction data.
    /// </summary>
    [Tooltip("Data for the Drowsiness affliction.")]
    public AfflictionData drowsinessData;

    /// <summary>
    /// Data for the Blurred Vision affliction. It is a <see cref="AfflictionData"/> that holds the affliction data.
    /// </summary>
    [Tooltip("Data for the Blurred Vision affliction.")]
    public AfflictionData blurredVisionData;

    /// <summary>
    /// Data for the Fatigue affliction. It is a <see cref="AfflictionData"/> that holds the affliction data.
    /// </summary>
    [Tooltip("Data for the Fatigue affliction.")]
    public AfflictionData fatigueData;

    #endregion

    #region Update Methods

    void Update()
    {
        UpdateAfflictions();
    }

    /// <summary>
    /// Method to update the active afflictions.
    /// It reduces the duration of each affliction and removes it if the duration reaches zero.
    /// </summary>
    private void UpdateAfflictions()
    {
        for (int i = activeAfflictions.Count - 1; i >= 0; i--)
        {
            // Reduce the duration of the affliction.
            activeAfflictions[i].Duration -= Time.deltaTime;

            // If the duration reaches zero, remove the affliction.
            if (activeAfflictions[i].Duration <= 0)
            {
                activeAfflictions[i].Remove();
                activeAfflictions.RemoveAt(i);
                Debug.Log("Affliction expired and removed.");
            }
        }
    }

    #endregion

    #region Queueing Methods

    /// <summary>
    /// Method to queue an affliction to the player.
    /// Because it is a static method, it can be called from anywhere in the code - even from other scripts.
    /// </summary>
    /// <param name="afflictionType"> It correlates the affliction type to its class and instatiates it. </param>
    public static void QueueAffliction(AfflictionType afflictionType)
    {
        // Instantiate the affliction based on the type.
        IAffliction newAffliction = null;

        // Switch case to instantiate the correct affliction type.
        switch (afflictionType)
        {
            case AfflictionType.Drowsiness:
                newAffliction = new DrowsinessAffliction(Instance.drowsinessData);
                break;
            case AfflictionType.BlurredVision:
                newAffliction = new BlurredVisionAffliction(Instance.blurredVisionData);
                break;
            case AfflictionType.Fatigue:
                newAffliction = new FatigueAffliction(Instance.fatigueData);
                break;
            default:
                Debug.LogError("Invalid affliction type.");
                break;
        }
        if (newAffliction != null)
        {
            // Queue the affliction to the player.
            Instance.QueueAffliction(newAffliction);
        }
    }

    /// <summary>
    /// Method to queue an affliction to the player internal to the class.
    /// </summary>
    /// <param name="newAffliction"> The affliction type to be queued. </param>
    public void QueueAffliction(IAffliction newAffliction)
    {
        // Check if the affliction already exists by matching Name.
        IAffliction existingAffliction = activeAfflictions.Find(a => a.Name == newAffliction.Name);

        // If the affliction already exists, extend its duration.
        if (existingAffliction != null)
        {
            existingAffliction.Duration += newAffliction.Duration;
            Debug.Log($"Extended duration of {newAffliction.Name} by {newAffliction.Duration} seconds.");
        }
        else // If the affliction does not exist, apply it.
        {
            newAffliction.Apply();
            activeAfflictions.Add(newAffliction);
            Debug.Log($"Added new affliction: {newAffliction.Name}.");
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Method to get the active afflictions on the player.
    /// </summary>
    /// <returns> Active afflictions. </returns>
    public List<IAffliction> GetActiveAfflictions()
    {
        return new List<IAffliction>(activeAfflictions);
    }

    /// <summary>
    /// Used to check, by the name of the affliction, if its applied to player.
    /// </summary>
    /// <param name="afflictionName"> The name of the affliction to check for. It can be either checked against the scriptable object name or just a string</param>
    /// <returns> <see cref="true"/> if it exists on player or <see cref="false"/> otherwise. </returns>
    public bool HasAffliction(string afflictionName)
    {
        return activeAfflictions.Exists(a => a.Name == afflictionName);
    }

    /// <summary>
    /// Used to return the affliction by its name.
    /// </summary>
    /// <param name="afflictionName"> The name of the affliction searched for. </param>
    /// <returns> The instance of the affliction, if available. </returns>
    public IAffliction ReturnAfflictionByName(string afflictionName)
    {
        return activeAfflictions.Find(a => a.Name == afflictionName);
    }

    #endregion
}