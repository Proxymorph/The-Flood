using UnityEngine;

/// <summary>
/// A mechanic to reduce anxiety by using earplugs.
/// The button is held down to activate the earplugs, and released to deactivate them.
/// The effectiveness of the earplugs decreases over time while active, and recovers over time when inactive.
/// </summary>
public class EarplugAnxietyFactor : MonoBehaviour, IAnxietyDownFactor
{
    #region Properties

    [Header("Earplug Settings")]

    /// <summary>
    /// Key to use the earplugs.
    /// </summary>
    [Tooltip("Key to use the earplugs.")]
    public KeyCode useKey = KeyCode.Q;

    /// <summary>
    ///  Maximum anxiety reduction per second, this is used as the baseline from which we decrease gradually the more use it has.
    /// </summary>
    [Tooltip("Maximum anxiety reduction per second, this is used as the baseline from which we decrease gradually the more use it has.")]
    public float maxAnxietyReduction = 10f;

    /// <summary>
    /// Rate at which effectiveness declines per second while active (value per second).
    /// </summary>
    [Tooltip("Rate at which effectiveness declines per second while active (value per second).")]
    public float effectivenessDeclineRate = 0.2f;

    /// <summary>
    /// Cooldown time (in seconds) after deactivation before recovery begins.
    /// </summary>
    [Tooltip("Cooldown time (in seconds) after deactivation before recovery begins.")]
    public float cooldownTime = 30f;

    /// <summary>
    /// Rate at which effectiveness recovers per second.
    /// </summary>
    [Tooltip("Rate at which effectiveness recovers per second.")]
    public float recoveryRate = 0.1f;

    [Header("Sound Dampening Settings")]

    /// <summary>
    /// Normal global audio volume.
    /// </summary>
    [Tooltip("Normal global audio volume.")]
    public float normalVolume = 1f;

    /// <summary>
    /// Volume when earplugs are active (simulate dampening).
    /// </summary>
    [Tooltip("Volume when earplugs are active (simulate dampening).")]
    public float earplugVolume = 0.3f;

    /// <summary>
    /// How quickly the volume transitions between states.
    /// </summary>
    [Tooltip("How quickly the volume transitions between states.")]
    public float volumeTransitionSpeed = 2f;

    /// <summary>
    /// Current effectiveness of the earplugs (0-1).
    /// An internal value that is used to calculate the actual anxiety reduction.
    /// </summary>
    private float currentEffectiveness = 1f;

    /// <summary>
    /// Whether earplugs are currently active.
    /// </summary>
    private bool earplugsActive = false;

    /// <summary>
    /// Cooldown timer.
    /// </summary>
    private float cooldownTimer = 0f;

    #endregion

    void Update()
    {
        // Toggle earplugs
        if (Input.GetKey(useKey))
        {
            ToggleEarplugs();
        }

        ApplyEarplugsDecreaseEffect();

        HandleAudioDampening();
    }

    #region Utility Methods

    /// <summary>
    /// Toggles the earplug state.
    /// </summary>
    private void ToggleEarplugs()
    {
        // Switch state.
        earplugsActive = !earplugsActive;

        if (earplugsActive)
        {
            AnxietyManager.Instance.RegisterDownFactor(this);

            // When activated, reset cooldown timer.
            cooldownTimer = cooldownTime;
            Debug.Log("Earplugs activated. Current effectiveness: " + currentEffectiveness);
        }
        else
        {
            AnxietyManager.Instance.UnregisterDownFactor(this);

            Debug.Log("Earplugs deactivated. Effectiveness will recover after cooldown.");
        }
    }

    /// <summary>
    /// Applies the decrease effect of the earplugs.
    /// </summary>
    private void ApplyEarplugsDecreaseEffect()
    {
        if (earplugsActive)
        {
            // While active, reduce effectiveness gradually.
            currentEffectiveness -= effectivenessDeclineRate * Time.deltaTime;
            currentEffectiveness = Mathf.Clamp01(currentEffectiveness);
            // Reset cooldown timer while earplugs are active.
            cooldownTimer = cooldownTime;
        }
        else
        {
            // If earplugs are not active, start cooldown.
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                // Once cooldown expires, recover effectiveness gradually.
                currentEffectiveness += recoveryRate * Time.deltaTime;
                currentEffectiveness = Mathf.Clamp01(currentEffectiveness);
            }
        }
    }

    /// <summary>
    /// Handles the audio dampening effect.
    /// </summary>
    private void HandleAudioDampening()
    {
        // Smoothly transition global audio volume.
        if (earplugsActive)
        {
            AudioListener.volume = Mathf.Lerp(AudioListener.volume, earplugVolume, volumeTransitionSpeed * Time.deltaTime);
        }
        else
        {
            AudioListener.volume = Mathf.Lerp(AudioListener.volume, normalVolume, volumeTransitionSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region IAnxietyDownFactor Implementation

    /// <summary>
    /// Returns the amount of anxiety reduction per frame.
    /// </summary>
    public float AnxietyDecrease()
    {
        // When active, return reduction modulated by effectiveness.
        return earplugsActive ? maxAnxietyReduction * currentEffectiveness : 0f;
    }

    #endregion
}