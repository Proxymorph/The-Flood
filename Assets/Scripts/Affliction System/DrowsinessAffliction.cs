using UnityEngine;

/// <summary>
/// This affliction is affecting the players stamina.
/// It will reduce it by a percentage, making every move count.
/// </summary>
public class DrowsinessAffliction : MonoBehaviour, IAffliction
{
    #region Properties

    /// <summary>
    /// The data for this affliction based on the scriptable object <see cref="AfflictionData"/>.
    /// </summary>
    private AfflictionData data;

    /// <summary>
    /// The display name of the affliction.
    /// </summary>
    public string Name => data.afflictionName;

    /// <summary>
    /// The icon to show on screen. Managed by the <see cref="AfflictionUIManager"/>.
    /// </summary>
    public Sprite Icon => data.icon;

    /// <summary>
    /// How long the affliction will affect the player (in seconds).
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    /// The stamina meter to be affected by the affliction.
    /// </summary>
    private StaminaMeter staminaMeter;

    #endregion

    #region Instancing

    /// <summary>
    /// Creates a new instance of the drowsiness affliction.
    /// This will be queued in the <see cref="AfflictionManager"/> and applied when it reaches the front of the queue.
    /// </summary>
    /// <param name="drowsinessData"> The pre-established data of the instantiated affliction. </param>
    /// <param name="duration"> Internal to the method checking duration. Can be set manually, or otherwise will be extracted from the data. </param>
    public DrowsinessAffliction(AfflictionData drowsinessData, float duration = -1f)
    {
        data = drowsinessData;

        // If the duration is not set, use the base duration from the data.
        Duration = (duration > 0) ? duration : drowsinessData.baseDuration;
    }

    #endregion

    #region Affliction Logic

    /// <summary>
    /// Applies the affliction to the player.
    /// It contains the custom logic for the drowsiness affliction application.
    /// </summary>
    public void Apply()
    {
        Debug.Log("Drowsiness applied");

        if (staminaMeter == null)
        {
            staminaMeter = FindFirstObjectByType<StaminaMeter>();
        }

        // Reduce the max stamina by 50%.
        if (staminaMeter != null)
        {
            staminaMeter.maxStamina *= 0.5f;
        }
    }

    /// <summary>
    /// Removes the affliction from the player.
    /// Contains the custom logic for the drowsiness affliction removal.
    /// </summary>
    public void Remove()
    {
        Debug.Log("Drowsiness removed");

        // Reset the max stamina to its original value.
        staminaMeter.maxStamina = 100f;
    }

    #endregion
}