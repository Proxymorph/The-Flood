using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Triggers a safe zone for breathing exercises when the player enters the zone.
/// </summary>
public class SafeZoneTrigger : MonoBehaviour
{
    #region Properties

    [Header("Safe Zone Settings")]

    /// <summary>
    /// Optional label to display the location name.
    /// </summary>
    [Tooltip("Optional label to display the location name.")]
    public string locationLabel = "Safe Zone Location";

    /// <summary>
    /// Radius within which the player is considered to be in a safe zone for breathing exercises.
    /// </summary>
    [Tooltip("Radius within which the player is considered to be in a safe zone for breathing exercises.")]
    public float safeZoneRadius = 5f;

    /// <summary>
    /// Optional: assign the player transform manually. Otherwise, the script will try to find a GameObject tagged 'Player'.
    /// </summary>
    [Tooltip("Optional: assign the player transform manually. Otherwise, the script will try to find a GameObject tagged 'Player'.")]
    public Transform player;

    /// <summary>
    /// Reference to the AudioSource component for playing audio when the player enters the safe zone.
    /// </summary>
    [Tooltip("Reference to the AudioSource component for playing audio when the player enters the safe zone.")]
    public AudioSource audioSource;

    /// <summary>
    /// Reference to the BreathingExerciseFactor component on the player.
    /// </summary>
    private BreathingExerciseFactor breathingExercise;

    #endregion

    void Start()
    {
        // If no player is manually assigned, try to find one by tag.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if(audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        // Attempt to get the BreathingExercise component.
        if (breathingExercise == null)
        {
            breathingExercise = FindFirstObjectByType<BreathingExerciseFactor>();
        }
    }

    void Update()
    {
        // If no player is manually assigned, try to find one by tag.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        UpdateBreathingFlag();
    }

    #region Utiliy Methods

    /// <summary>
    /// Update the breathing exercise flag based on the player's proximity to the safe zone.
    /// </summary>
    private void UpdateBreathingFlag()
    {
        if (player == null || breathingExercise == null)
            return;

        // Check the distance between the player and this safe zone.
        float distance = Vector3.Distance(player.position, transform.position);
        // Update the breathing exercise flag based on proximity.
        breathingExercise.isInSafeZone = (distance <= safeZoneRadius);

        // Play the audio source if the player is in the safe zone.
        if (breathingExercise.isInSafeZone && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!breathingExercise.isInSafeZone && audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        // Draw a green wire sphere representing the safe zone radius.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeZoneRadius);

#if UNITY_EDITOR
        // Draw the label above the sphere.
        Handles.Label(transform.position + Vector3.up * (safeZoneRadius + 0.5f), locationLabel);
#endif
    }

    private void OnValidate()
    {
        // Ensure the Scene view updates when properties change in the Inspector.
#if UNITY_EDITOR
        SceneView.RepaintAll();
#endif
    }

    #endregion
}