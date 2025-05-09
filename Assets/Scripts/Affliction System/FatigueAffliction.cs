using Gameplay;
using UnityEngine;

/// <summary>
/// This affliction is affecting the players jumping ability.
/// The jumping power is cut by a percentage, making the parkour harder.
/// </summary>
public class FatigueAffliction : MonoBehaviour, IAffliction
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
    /// The player character to be affected by the affliction.
    /// It contains the jumping force property to be affected.
    /// </summary>
    private PlayerCharacter player;

    #endregion

    #region Instancing

    /// <summary>
    /// Creates a new instance of the fatigue affliction.
    /// This will be queued in the <see cref="AfflictionManager"/> and applied when it reaches the front of the queue.
    /// </summary>
    /// <param name="fatigueData"> The pre-established data of the instantiated affliction. </param>
    /// <param name="duration"> Internal to the method checking duration. Can be set manually, or otherwise will be extracted from the data. </param>
    public FatigueAffliction(AfflictionData fatigueData, float duration = -1f)
    {
        data = fatigueData;

        // If the duration is not set, use the base duration from the data.
        Duration = (duration > 0) ? duration : fatigueData.baseDuration;
    }

    #endregion

    #region Affliction Logic

    /// <summary>
    /// Applies the affliction to the player.
    /// It contains the custom logic for the fatigue affliction application.
    /// </summary>
    public void Apply()
    {
        Debug.Log($"{Name} applied");

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerCharacter>();

            player.jumping.force *= 0.5f;
        }
        else
        {
            // If the player is already affected by the affliction, reduce the jumping force by 50%.
            player.jumping.force *= 0.5f;
        }
    }

    /// <summary>
    /// Removes the affliction from the player.
    /// Contains the custom logic for the fatigue affliction removal.
    /// </summary>
    public void Remove()
    {
        Debug.Log($"{Name} removed");

        // Reset the jumping force to the base value.
        player.jumping.force = 1f;
    }

    #endregion
}
