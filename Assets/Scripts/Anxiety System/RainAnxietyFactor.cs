using UnityEngine;

/// <summary>
/// Rain anxiety factor increases anxiety when it's raining. 
/// The intensity of the rain affects the rate of increase.
/// It implements the <see cref="IAnxietyUpFactor"/> interface.
/// </summary>
public class RainAnxietyFactor : MonoBehaviour, IAnxietyUpFactor
{
    #region Properties

    /// <summary>
    /// Base rate of anxiety increase in rain.
    /// </summary>
    [Tooltip("Base rate of anxiety increase in rain.")]

    public float baseIncrease = 0.3f;

    /// <summary>
    /// Intensity of the rain. Set by the <see cref="WeatherSystem"/>.
    /// </summary>
    [Tooltip("Intensity of the rain. Set by the WeatherSystem.")]
    [Range(0f, 1f)]
    public float rainIntensity = 1f;

    #endregion

    private void Start()
    {
        // Register this factor with the Anxiety Manager. It rain constantly, so it's always active.
        AnxietyManager.Instance.RegisterUpFactor(this);
    }

    private void Update()
    {
        // If the WeatherSystem is not present, return.
        if (WeatherSystem.Instance == null)
        {
            return;
        }
        else // Otherwise, update the rain intensity.
        {
            rainIntensity = WeatherSystem.Instance.currentRain;
        }

        // If the rain intensity is 0, unregister this factor.
        if (rainIntensity == 0)
        {
            AnxietyManager.Instance.UnregisterUpFactor(this);
        }
        else // Otherwise, register it.
        {
            AnxietyManager.Instance.RegisterUpFactor(this);
        }
    }

    #region IAnxietyUpFactor Implementation

    public float AnxietyIncrease()
    {
        if(rainIntensity == 0)
        {
            return 0;
        }

        // Multiply base rate by intensity.
        return baseIncrease * rainIntensity;
    }

    #endregion
}
