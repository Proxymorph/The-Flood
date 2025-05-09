using System.Collections;
using UnityEngine;

/// <summary>
/// A factor that allows the player to reduce anxiety by performing a breathing exercise.
/// </summary>
public class BreathingExerciseFactor : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// The character controller component of the player.
    /// </summary>
    private CharacterController characterController;

    [Header("Breathing Exercise Settings")]

    /// <summary>
    /// The key to start the exercise.
    /// </summary> 
    [Tooltip("Key to start the exercise.")]
    public KeyCode startKey = KeyCode.B;

    /// <summary>
    /// The base rate at which anxiety is reduced when the exercise is fully effective.
    /// </summary>
    [Tooltip("Maximum reduction per second when fully effective.")]
    public float baseAnxietyReductionRate = 10f;

    /// <summary>
    /// The rate at which the effectiveness of the exercise declines over time.
    /// </summary>
    [Tooltip("How fast effectiveness declines per second.")]
    public float effectivenessDeclineRate = 0.1f;

    /// <summary>
    /// The minimum effectiveness allowed for the exercise.
    /// </summary>
    [Tooltip("Lowest effectiveness allowed.")]
    public float minimumEffectiveness = 0.2f;

    /// <summary>
    /// The cooldown time (in seconds) after finishing the exercise.
    /// </summary>
    [Tooltip("Cooldown time (in seconds) after finishing exercise.")]
    public float cooldownTime = 10f;

    [Header("Safe Zone Requirement")]

    /// <summary>
    /// If true, the exercise can only be performed in safe zones.
    /// </summary>
    [Tooltip("If true, exercise can only be performed in safe zones.")]
    public bool safeZoneRequired = true;

    /// <summary>
    /// Whether the player is currently in a safe zone.
    /// </summary>
    [Tooltip("Should be set externally (e.g., via triggers).")]
    public bool isInSafeZone = false;

    /// <summary>
    /// Whether the player is currently performing the exercise.
    /// </summary>
    private bool isExercising = false;

    /// <summary>
    /// The current effectiveness of the exercise.
    /// </summary>
    private float currentEffectiveness = 1f;

    /// <summary>
    /// The cooldown timer for the exercise.
    /// </summary>
    private float cooldownTimer = 0f;

    #endregion

    void Start()
    {
        if(characterController == null)
        {
            characterController = FindFirstObjectByType<CharacterController>();
        }
    }

    void Update()
    {
        if (characterController == null)
        {
            characterController = FindFirstObjectByType<CharacterController>();
        }

        TryToUseBreathingExercise();
    }

    #region Utility Methods

    /// <summary>
    /// Used to manage the breathing exercise state.
    /// If the player is not currently exercising and the cooldown is finished, the exercise can be started.
    /// </summary>
    private void TryToUseBreathingExercise()
    {
        // If not exercising and cooldown is finished, allow starting the exercise.
        if (!isExercising && cooldownTimer <= 0f)
        {
            if (Input.GetKeyDown(startKey))
            {
                if (!safeZoneRequired || (safeZoneRequired && isInSafeZone))
                {
                    StartCoroutine(BreathingRoutine());
                }
                else
                {
                    Debug.Log("Breathing exercise can only be used in a safe zone.");
                }
            }
        }
        else if (cooldownTimer > 0f)
        {
            // Cooldown is active: recover effectiveness over time.
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                currentEffectiveness = 1f; // Reset to full effectiveness.
                Debug.Log("Breathing exercise effectiveness reset.");
            }
        }
    }

    /// <summary>
    /// Coroutine that handles the breathing exercise.
    /// </summary>
    /// <returns> It reduces the anxiety as long as the exercise is performed. </returns>
    private IEnumerator BreathingRoutine()
    {
        isExercising = true;
        Debug.Log("Breathing exercise started. Player is locked in place.");

        // Disable player movement while breathing.
        characterController.enabled = false;

        // While the key is held, continuously reduce anxiety.
        while (Input.GetKey(startKey))
        {
            // Reduce anxiety: negative value lowers the meter.
            AnxietyManager.Instance.ModifyAnxiety(-baseAnxietyReductionRate * currentEffectiveness * Time.deltaTime);

            // Gradually decrease the effectiveness the longer the breath is held.
            currentEffectiveness -= effectivenessDeclineRate * Time.deltaTime;
            currentEffectiveness = Mathf.Clamp(currentEffectiveness, minimumEffectiveness, 1f);

            yield return null;
        }

        Debug.Log("Breathing exercise ended. Starting cooldown.");

        // Re-enable player movement after breathing.
        characterController.enabled = true;

        isExercising = false;
        cooldownTimer = cooldownTime;
    }

    #endregion
}
