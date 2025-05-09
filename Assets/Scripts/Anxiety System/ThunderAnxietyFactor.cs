using UnityEngine;

/// <summary>
/// This class represents a factor that increases anxiety when thunder occurs.
/// It implements the <see cref="IAnxietyUpFactor"/> interface.
/// </summary>
public class ThunderAnxietyFactor : MonoBehaviour, IAnxietyUpFactor
{
    #region Properties

    /// <summary>
    /// The amount by which the anxiety increases when thunder occurs.
    /// </summary>
    [Tooltip("The amount by which the anxiety increases when thunder occurs.")]
    public float spikeIncrease = 20f;

    /// <summary>
    /// The duration for which the spike remains active.
    /// </summary>
    [Tooltip("The duration for which the spike remains active.")]
    public float spikeDuration = 1f;

    /// <summary>
    /// A flag to indicate if the thunder has been triggered.
    /// </summary>
    private bool thunderTriggered = false;

    /// <summary>
    /// The timer for the spike duration.
    /// </summary>
    private float spikeTimer = 0f;

    #endregion

    void Update()
    {
        TestThunder();
        HandleThunder();
    }

    #region Utility

    /// <summary>
    /// Handles the thunder event.
    /// </summary>
    private void HandleThunder()
    {
        // If the thunder is active and the spike timer is not active, trigger the spike.
        if (WeatherSystem.Instance.isLightningActive && spikeTimer == 0)
        {
            thunderTriggered = true;

            // Register this factor with the Anxiety Manager.
            AnxietyManager.Instance.RegisterUpFactor(this);

            spikeTimer = spikeDuration;
        }

        // If the spike is active, decrement the timer.
        if (thunderTriggered)
        {
            spikeTimer -= Time.deltaTime;
            if (spikeTimer <= 0f)
            {
                // Unregister this factor with the Anxiety Manager.
                AnxietyManager.Instance.UnregisterUpFactor(this);

                thunderTriggered = false;
            }
        }
    }

    /// <summary>
    /// For testing: press 'T' to simulate thunder.
    /// </summary>
    private void TestThunder()
    {
        // For testing: press 'T' to simulate thunder.
        if (Input.GetKeyDown(KeyCode.T))
        {
            thunderTriggered = true;

            // Register this factor with the Anxiety Manager.
            AnxietyManager.Instance.RegisterUpFactor(this);

            spikeTimer = spikeDuration;
        }
    }

    #endregion

    #region IAnxietyUpFactor Implementation

    public float AnxietyIncrease()
    {
        // If the thunder is triggered, return the spike increase value.
        return thunderTriggered ? spikeIncrease : 0f;
    }

    #endregion
}
