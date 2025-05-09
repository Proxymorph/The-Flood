using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The manager for the anxiety system.
/// It handles the anxiety level, applying uo and down factors and effects.
/// </summary>
public class AnxietyManager : MonoBehaviour
{
    #region Singleton

    /// <summary>
    /// The singleton instance of the anxiety manager.
    /// </summary>
    public static AnxietyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple AnxietyManagers detected. Destroying the new one on " + gameObject.name);
            Destroy(gameObject);
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// The slider UI element that represents the anxiety level.
    /// </summary>
    private Slider anxietySlider;

    [Header("Anxiety Settings")]

    /// <summary>
    /// The maximum anxiety level.
    /// </summary>
    [Tooltip("The maximum anxiety level.")]
    public float maxAnxiety = 100f;

    /// <summary>
    /// The current anxiety level.
    /// </summary>
    [Tooltip("The current anxiety level.")]
    public float currentAnxiety = 0f;

    [Header("Thresholds")]

    /// <summary>
    /// The threshold for level 1 anxiety.
    /// </summary>
    [Tooltip("The threshold for level 1 anxiety.")]
    public float level1Threshold = 30f;

    /// <summary>
    /// The threshold for level 2 anxiety.
    /// </summary>
    [Tooltip("The threshold for level 2 anxiety.")]
    public float level2Threshold = 60f;

    /// <summary>
    /// The threshold for level 3 anxiety.
    /// </summary>
    [Tooltip("The threshold for level 3 anxiety.")]
    public float level3Threshold = 90f;

    [Header("Affliction Chances")]

    [Header("Level 1 Chance")]

    /// <summary>
    /// The chance to apply fatigue at level 1 anxiety.
    /// </summary>
    [Tooltip("The chance to apply fatigue at level 1 anxiety.")]
    public float chanceFatigueLevel1 = 0.3f;

    [Header("Level 2 Chances")]

    /// <summary>
    /// The chance to apply fatigue at level 2 anxiety.
    /// </summary>
    [Tooltip("The chance to apply fatigue at level 2 anxiety.")]
    public float chanceFatigueLevel2 = 0.5f;

    /// <summary>
    /// The chance to apply drowsiness at level 2 anxiety.
    /// </summary>
    [Tooltip("The chance to apply drowsiness at level 2 anxiety.")]
    public float chanceDrowsinessLevel2 = 0.3f;

    [Header("Level 3 Chances")]

    /// <summary>
    /// The chance to apply fatigue at level 3 anxiety.
    /// </summary>
    [Tooltip("The chance to apply fatigue at level 3 anxiety.")]
    public float chanceFatigueLevel3 = 0.7f;

    /// <summary>
    /// The chance to apply drowsiness at level 3 anxiety.
    /// </summary>
    [Tooltip("The chance to apply drowsiness at level 3 anxiety.")]
    public float chanceDrowsinessLevel3 = 0.5f;

    /// <summary>
    /// The chance to apply blurred vision at level 3 anxiety.
    /// </summary>
    [Tooltip("The chance to apply blurred vision at level 3 anxiety.")]
    public float chanceBlurredVisionLevel3 = 0.3f;

    /// <summary>
    /// The list of active up factors that increase anxiety.
    /// </summary>
    private List<IAnxietyUpFactor> activeUpFactors = new List<IAnxietyUpFactor>();

    /// <summary>
    /// The list of active down factors that decrease anxiety.
    /// </summary>
    private List<IAnxietyDownFactor> activeDownFactors = new List<IAnxietyDownFactor>();

    #endregion

    private void Start()
    {
        // Find the anxiety slider if it's not set.
        if (anxietySlider == null)
        {
            anxietySlider = GameObject.Find("AnxietySlider").GetComponent<Slider>();
        }
    }

    private void Update()
    {
        AnxietyMeterUpdater();
        ApplyAnxietyModifiers();
        HandleAnxietyEffects();
    }

    #region UI Methods

    /// <summary>
    /// Updates the anxiety meter UI element and changes its color based on the anxiety level.
    /// </summary>
    private void AnxietyMeterUpdater()
    {
        anxietySlider.value = currentAnxiety / maxAnxiety;

        // Change color based on anxiety level
        if (anxietySlider.value >= level2Threshold / maxAnxiety)
        {
            anxietySlider.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if (anxietySlider.value >= level1Threshold / maxAnxiety)
        {
            anxietySlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            anxietySlider.fillRect.GetComponent<Image>().color = Color.blue;
        }
    }

    #endregion

    #region Anxiety Logic

    /// <summary>
    /// Applies the anxiety modifiers based on the active factors.
    /// </summary>
    private void ApplyAnxietyModifiers()
    {
        float upTotal = 0f;
        foreach (var up in activeUpFactors)
        {
            float increase = up.AnxietyIncrease();
            upTotal += increase;
            currentAnxiety += increase * Time.deltaTime;
        }

        float downTotal = 0f;
        foreach (var down in activeDownFactors)
        {
            float decrease = down.AnxietyDecrease();
            downTotal += decrease;
            currentAnxiety -= decrease * Time.deltaTime;
        }

        //Debug.Log($"Anxiety Change: +{upTotal * Time.deltaTime}, -{downTotal * Time.deltaTime} | Active Up Factors: {activeUpFactors.Count}");

        currentAnxiety = Mathf.Clamp(currentAnxiety, 0, maxAnxiety);
    }

    /// <summary>
    /// Helper method that tries to apply an affliction based on a chance.
    /// </summary>
    /// <param name="affliction"> The instance of the affliction to be applied. </param>
    /// <param name="chance"> Chance to apply affliction. Set manually or via a pre-set threshold.</param>
    private void TryApplyAffliction(IAffliction affliction, float chance)
    {
        if (Random.value < chance)
        {
            // Avoid duplicates.
            if (!AfflictionManager.Instance.HasAffliction(affliction.Name))
            {
                Debug.Log($"Applying {affliction.Name} with a chance of {chance * 100}%");

                AfflictionManager.Instance.QueueAffliction(affliction);
            }
        }
    }

    /// <summary>
    /// Handles the application of anxiety effects based on the current anxiety level.
    /// </summary>
    private void HandleAnxietyEffects()
    {
        // Get the active afflictions to avoid duplicates.
        var existingAffliction = AfflictionManager.Instance.GetActiveAfflictions();

        // Apply afflictions based on anxiety level.
        if (currentAnxiety >= level3Threshold)
        {
            TryApplyAffliction(new FatigueAffliction(AfflictionManager.Instance.fatigueData), chanceFatigueLevel3);
            TryApplyAffliction(new DrowsinessAffliction(AfflictionManager.Instance.drowsinessData), chanceDrowsinessLevel3);
            TryApplyAffliction(new BlurredVisionAffliction(AfflictionManager.Instance.blurredVisionData), chanceBlurredVisionLevel3);
        }
        else if (currentAnxiety >= level1Threshold && currentAnxiety <= level2Threshold)
        {
            TryApplyAffliction(new FatigueAffliction(AfflictionManager.Instance.fatigueData), chanceFatigueLevel2);
            TryApplyAffliction(new DrowsinessAffliction(AfflictionManager.Instance.drowsinessData), chanceDrowsinessLevel2);
        }
        else if (currentAnxiety <= level1Threshold)
        {
            TryApplyAffliction(new FatigueAffliction(AfflictionManager.Instance.fatigueData), chanceFatigueLevel1);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Registers an up factor to increase anxiety.
    /// </summary>
    /// <param name="factor"> The instance of the up factor to be registered.</param>
    public void RegisterUpFactor(IAnxietyUpFactor factor)
    {
        // Avoid duplicates.
        if (!activeUpFactors.Contains(factor))
        {
            activeUpFactors.Add(factor);
        }
    }

    /// <summary>
    /// Registers a down factor to decrease anxiety.
    /// </summary>
    /// <param name="factor"> The instance of the down factor to be registered. </param>
    public void RegisterDownFactor(IAnxietyDownFactor factor)
    {
        // Avoid duplicates.
        if (!activeDownFactors.Contains(factor))
        {
            activeDownFactors.Add(factor);
        }
    }

    /// <summary>
    /// Unregisters an up factor to stop increasing anxiety.
    /// </summary>
    /// <param name="factor"> The instance of the up factor to be unregistered.</param>
    public void UnregisterUpFactor(IAnxietyUpFactor factor)
    {
        activeUpFactors.Remove(factor);
    }

    /// <summary>
    /// Unregisters a down factor to stop decreasing anxiety.
    /// </summary>
    /// <param name="factor"> The instance of the down factor to be unregistered.</param>
    public void UnregisterDownFactor(IAnxietyDownFactor factor)
    {
        activeDownFactors.Remove(factor);
    }

    #endregion

    #region External Modifications

    /// <summary>
    /// Modifies the current anxiety level by a given amount from external sources.
    /// </summary>
    /// <param name="amount"> The ammount to modify by. </param>
    public void ModifyAnxiety(float amount)
    {
        currentAnxiety = Mathf.Clamp(currentAnxiety + amount, 0, maxAnxiety);
    }

    /// <summary>
    /// Returns the current anxiety level.
    /// </summary>
    /// <returns> Current anxiety level. </returns>
    public float GetCurrentAnxiety() => currentAnxiety;

    #endregion
}