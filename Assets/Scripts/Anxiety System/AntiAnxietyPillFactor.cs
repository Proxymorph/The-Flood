using UnityEngine;

/// <summary>
/// A factor that allows the player to reduce anxiety by using anti-anxiety pills.
/// If overused within a certain time window, the player will receive a penalty affliction.
/// </summary>
public class AntiAnxietyPillFactor : MonoBehaviour
{
    #region Properties

    [Header("Pill Settings")]

    /// <summary>
    /// The key to press to use the anti-anxiety pill.
    /// </summary>
    public KeyCode useKey = KeyCode.P;

    /// <summary>
    /// The amount by which anxiety is reduced each use
    /// </summary>
    [Tooltip("The amount by which anxiety is reduced each use.")]
    public float anxietyReductionAmount = -20f;

    /// <summary>
    /// Minimum time (in seconds) between pill uses
    /// </summary>
    [Tooltip("Minimum time (in seconds) between pill uses.")]
    public float cooldownBetweenUses = 10f;

    /// <summary>
    /// Overuse threshold: number of uses within the overuse window to trigger a penalty
    /// </summary>
    [Tooltip("Overuse threshold: number of uses within the overuse window to trigger a penalty.")]
    public int overuseThreshold = 3;

    /// <summary>
    /// The time window (in seconds) during which uses are counted toward overuse
    /// </summary>
    [Tooltip("The time window (in seconds) during which uses are counted toward overuse.")]
    public float overuseWindow = 30f;

    /// <summary>
    /// The time of the last pill use
    /// </summary>
    private float lastUseTime = -Mathf.Infinity;

    /// <summary>
    /// The number of pill uses within the overuse window
    /// </summary>
    private int useCount = 0;

    /// <summary>
    /// The time at which the current overuse window started
    /// </summary>
    private float windowStartTime = 0f;

    [Header("Affliction Chance Settings")]

    /// <summary>
    /// The chance of receiving the fatigue affliction when over
    /// </summary>
    private readonly float fatigueChance = 0.4f;

    /// <summary>
    /// The chance of receiving the drowsiness affliction when over
    /// </summary>
    private readonly float drowsinessChance = 0.3f;

    /// <summary>
    /// The chance of receiving the blurred vision affliction when over
    /// </summary>
    private readonly float blurredVisionChance = 0.2f;

    #endregion

    void Update()
    {
        // Check for pill use input
        if (Input.GetKeyDown(useKey))
        {
            TryUsePills();
        }
    }

    #region Utility Methods

    /// <summary>
    /// Attempts to use an anti-anxiety pill.
    /// </summary>
    private void TryUsePills()
    {
        // Enforce cooldown between pill uses.
        if (Time.time - lastUseTime < cooldownBetweenUses)
        {
            Debug.Log("Pills are on cooldown.");
        }

        lastUseTime = Time.time;

        // If we're outside the overuse window, reset the counter
        if (Time.time - windowStartTime > overuseWindow)
        {
            windowStartTime = Time.time;
            useCount = 0;
        }

        if(PillInventory.Instance.pillCount <= 0)
        {
            Debug.Log("No pills left to use.");
            return;
        }
        else
        {
            // Remove a pill from the inventory
            PillInventory.Instance.AddPills(-1);

            // Reduce anxiety by calling AnxietyManager's method
            AnxietyManager.Instance.ModifyAnxiety(anxietyReductionAmount);
        }

        useCount++;

        Debug.Log($"Anti-Anxiety Pill used. Current use count: {useCount}");

        // Check for overuse.
        if (useCount > overuseThreshold)
        {
            Debug.Log("Overuse of pills detected! Applying Pill Overuse affliction.");

            // Randomize the affliction to apply based on the set chances
            if(Random.value < fatigueChance)
            {
                AfflictionManager.Instance.QueueAffliction(new FatigueAffliction(AfflictionManager.Instance.fatigueData));
            }
            else if(Random.value < drowsinessChance)
            {
                AfflictionManager.Instance.QueueAffliction(new DrowsinessAffliction(AfflictionManager.Instance.drowsinessData));
            }
            else if (Random.value < blurredVisionChance)
            {
                AfflictionManager.Instance.QueueAffliction(new BlurredVisionAffliction(AfflictionManager.Instance.blurredVisionData));
            }

            // Reset use count after applying the penalty
            useCount = 0;
        }

    }

    #endregion
}
