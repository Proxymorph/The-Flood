using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// This affliction is affecting the players vision.
/// It will apply a blur effect to the screen. Temporarly imparing the player.
/// </summary>
public class BlurredVisionAffliction : MonoBehaviour, IAffliction
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
    /// The post process volume that contains the blurr effect <see cref="DepthOfField"/>.
    /// </summary>
    private Volume postProcessVolume;

    /// <summary>
    /// The depth of field effect to be modified.
    /// </summary>
    private DepthOfField dof;

    #endregion

    #region Instancing

    /// <summary>
    /// Creates a new instance of the blurred vision affliction.
    /// This will be queued in the <see cref="AfflictionManager"/> and applied when it reaches the front of the queue.
    /// </summary>
    /// <param name="blurredVisionData"> The pre-established data of the instantiated affliction. </param>
    /// <param name="duration"> Internal to the method checking duration. Can be set manually, or otherwise will be extracted from the data. </param>
    public BlurredVisionAffliction(AfflictionData blurredVisionData, float duration = -1f)
    {
        data = blurredVisionData;

        // If the duration is not set, use the base duration from the data.
        Duration = (duration > 0) ? duration : blurredVisionData.baseDuration;
    }

    #endregion

    #region Affliction Logic

    /// <summary>
    /// Applies the affliction to the player.
    /// It contains the custom logic for the blurred vision affliction application.
    /// </summary>
    public void Apply()
    {
        Debug.Log($"{Name} applied");

        // Find the post process volume in the scene.
        if (postProcessVolume == null)
        {
            postProcessVolume = GameObject.Find("Scene Settings").GetComponent<Volume>();

            // Get the depth of field effect from the post process volume.
            if (postProcessVolume.profile.TryGet<DepthOfField>(out DepthOfField tempDoF))
            {
                dof = tempDoF;

                // Set the focus mode to manual to allow for custom focus distance.
                // The settings are already set in the post process volume for the manual mode.
                dof.focusMode.value = DepthOfFieldMode.Manual;
            }
        }
    }

    public void Remove()
    {
        Debug.Log($"{Name} removed");

        if (dof != null)
        {
            // Reset the focus mode to use the physical camera settings.
            dof.focusMode.value = DepthOfFieldMode.UsePhysicalCamera;
        }
    }

    #endregion
}
