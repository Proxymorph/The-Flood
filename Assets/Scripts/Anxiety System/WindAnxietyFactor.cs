using UnityEngine;

/// <summary>
/// An anxiety factor that increases anxiety based on the wind speed.
/// Implements the <see cref="IAnxietyUpFactor"/> interface.
/// </summary>
public class WindAnxietyFactor : MonoBehaviour, IAnxietyUpFactor
{
    #region Properties

    /// <summary>
    /// Reference to the <see cref="WindControl"/> script in the scene.
    /// </summary>
    private WindControl windControl;

    /// <summary>
    /// Current wind speed (Set by the <see cref="WindControl"/> script).
    /// </summary>
    [Tooltip("Current wind speed (Set by the WindControl script).")]
    public float windSpeed;

    /// <summary>
    /// Wind speed threshold above which anxiety starts to increase.
    /// </summary>
    [Tooltip("Wind speed threshold above which anxiety starts to increase.")]
    public float windThreshold = 1f;

    /// <summary>
    /// Maximum wind speed for scaling.
    /// </summary>
    [Tooltip("Maximum wind speed for scaling.")]
    public float maxWindSpeed = 30f;

    /// <summary>
    /// Base anxiety increase rate.
    /// </summary>
    [Tooltip("Base anxiety increase rate.")]
    public float baseIncrease = 4f;

    #endregion

    private void Start()
    {
        // Find the WindControl script in the scene.
        windControl = FindFirstObjectByType<WindControl>();

        if (windControl == null)
        {
            Debug.LogError("WindControl script not found in the scene. Please attach it to a game object.");
            return;
        }

        // Register this factor with the Anxiety Manager.
        AnxietyManager.Instance.RegisterUpFactor(this);
    }

    private void Update()
    {
        ModifyWindBasedAnxiety();
    }

    #region Utility

    /// <summary>
    /// Finds the <see cref="WeatherSystem"/> in the scene.
    /// Updates the internal wind speed and anxiety based on the weather system wind speed.
    /// </summary>
    private void ModifyWindBasedAnxiety()
    {
        if (WeatherSystem.Instance == null)
        {
            return;
        }
        else
        {
            windSpeed = WeatherSystem.Instance.currentWind;
        }

        // Apply the internal wind speed to the WindControl script.
        windControl.windGlobalStrengthScale = windSpeed;

        // Register or unregister this factor based on the wind speed threshold.
        if (windSpeed < windThreshold)
        {
            AnxietyManager.Instance.UnregisterUpFactor(this);
        }
        else if (windSpeed > windThreshold)
        {
            AnxietyManager.Instance.RegisterUpFactor(this);
        }
    }

    #endregion

    #region IAnxietyUpFactor Implementation

    public float AnxietyIncrease()
    {
        if (windSpeed < windThreshold)
        {
            return 0f;
        }

        float normalized = Mathf.Clamp01((windSpeed - windThreshold) / (maxWindSpeed - windThreshold));
        return baseIncrease * normalized;
    }

    #endregion
}
