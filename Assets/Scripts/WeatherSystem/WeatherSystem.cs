using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// A simple weather system that controls wind, rain, and lightning effects.
/// </summary>
public class WeatherSystem : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// Singleton instance of the WeatherSystem.
    /// </summary>
    public static WeatherSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple WeatherSystems detected. Destroying the new one on " + gameObject.name);
            Destroy(gameObject);
        }
    }

    #endregion

    #region Properties

    [Header("Player Reference")]

    /// <summary>
    /// Player transform to spawn lightning in front of.
    /// </summary>
    [Tooltip("Player transform to spawn lightning in front of.")]
    public Transform player;

    [Header("Lightning & Thunder Settings")]

    /// <summary>
    /// Prefab for the lightning effect.
    /// </summary>
    [Tooltip("Prefab for the lightning effect.")]
    public GameObject lightningPrefab;

    /// <summary>
    /// Thunder sound clip.
    /// </summary>
    [Tooltip("Thunder sound clip.")]
    public AudioClip thunderSound;

    /// <summary>
    /// AudioSource to play the thunder.
    /// </summary>
    [Tooltip("AudioSource to play the thunder.")]
    public AudioSource audioSource;

    /// <summary>
    /// Distance in front of the player to spawn lightning.
    /// </summary>
    [Tooltip("Distance in front of the player to spawn lightning.")]
    public float lightningDistance = 15f;

    /// <summary>
    /// Minimum interval between lightning strikes.
    /// </summary>
    [Tooltip("Minimum interval between lightning strikes.")]
    public float minLightningInterval = 10f;

    /// <summary>
    /// Maximum interval between lightning strikes.
    /// </summary>
    [Tooltip("Maximum interval between lightning strikes.")]
    public float maxLightningInterval = 30f;

    /// <summary>
    /// Random offset on X and Z for the lightning spawn position.
    /// </summary>
    [Tooltip("Random offset on X and Z for the lightning spawn position.")]
    public Vector2 lightningOffset = new Vector2(2f, 2f);

    /// <summary>
    /// Is lightning currently active?
    /// </summary>
    [Tooltip("Is lightning currently active?")]
    [HideInInspector]
    public bool isLightningActive;

    [Header("Wind Settings")]

    /// <summary>
    /// Minimum wind intensity.
    /// </summary>
    [Tooltip("Minimum wind intensity.")]
    public float windMin = 0.8f;

    /// <summary>
    /// Maximum wind intensity.
    /// </summary>
    [Tooltip("Maximum wind intensity.")]
    public float windMax = 3f;

    /// <summary>
    /// Seconds to reach new target wind intensity.
    /// </summary>
    [Tooltip("Seconds to reach new target wind intensity.")]
    public float windTransitionDuration = 10f;

    /// <summary>
    /// Current wind intensity.
    /// </summary>
    [Tooltip("Current wind intensity.")]
    [HideInInspector] 
    public float currentWind;

    [Header("Rain Settings")]

    /// <summary>
    /// VisualEffectAsset for the rain effect.
    /// </summary>
    private VisualEffectAsset rainEffect;

    /// <summary>
    /// Minimum rain intensity (0 to 1).
    /// </summary>
    [Tooltip("Minimum rain intensity (0 to 1).")]
    public float rainMin = 0f;

    /// <summary>
    /// Maximum rain intensity (0 to 1).
    /// </summary>
    [Tooltip("Maximum rain intensity (0 to 1).")]
    public float rainMax = 1f;

    /// <summary>
    /// Seconds to reach new target rain intensity.
    /// </summary>
    [Tooltip("Seconds to reach new target rain intensity.")]
    public float rainTransitionDuration = 10f;

    /// <summary>
    /// Current rain intensity.
    /// </summary>
    [Tooltip("Current rain intensity.")]
    [HideInInspector]
    public float currentRain;

    [Header("Rain Audio Settings")]

    /// <summary>
    /// AudioSource for continuous rain sound.
    /// </summary>
    [Tooltip("AudioSource for continuous rain sound.")]
    public AudioSource rainAudioSource;

    /// <summary>
    /// Minimum volume for the rain audio.
    /// </summary>
    [Tooltip("Minimum volume for the rain audio.")]
    public float minRainVolume = 0.1f;

    /// <summary>
    /// Maximum volume for the rain audio.
    /// </summary>
    [Tooltip("Maximum volume for the rain audio.")]
    public float maxRainVolume = 1f;

    /// <summary>
    /// Minimum volume for the rain audio.
    /// </summary>
    private float targetWind;

    /// <summary>
    /// Target wind intensity.
    /// </summary>
    private float targetRain;

    /// <summary>
    /// Is the player currently indoors?
    /// </summary>
    [HideInInspector]
    public bool isIndoor = false;


    #endregion

    void Start()
    {
        if (player == null)
        {
            player = Camera.main.transform;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Initialize wind.
        currentWind = Random.Range(windMin, windMax);
        targetWind = Random.Range(windMin, windMax);

        // Initialize rain.
        currentRain = Random.Range(rainMin, rainMax);
        targetRain = Random.Range(rainMin, rainMax);

        if(rainEffect == null)
        {
            rainEffect = Resources.Load<VisualEffectAsset>("RainEffect");
        }

        // Configure and start the rain audio.
        if (rainAudioSource != null)
        {
            rainAudioSource.loop = true;
            float normalizedRain = Mathf.InverseLerp(rainMin, rainMax, currentRain);
            rainAudioSource.volume = Mathf.Lerp(minRainVolume, maxRainVolume, normalizedRain);
            if (!rainAudioSource.isPlaying)
                rainAudioSource.Play();
        }

        // Start the lightning routine.
        StartCoroutine(LightningRoutine());
    }

    void Update()
    {
        if (audioSource.isPlaying && isIndoor == false)
        {
            isLightningActive = true;
        }
        else
        {
            isLightningActive = false;
        }

        if(isIndoor == false)
        {
            UpdateWind();
            UpdateRain();
        }
    }

    #region Utility Methods

    /// <summary>
    /// Set the wind intensity.
    /// </summary>
    void UpdateWind()
    {
        float windDelta = Mathf.Abs(targetWind - currentWind);
        float windChangeRate = (windDelta / windTransitionDuration);
        currentWind = Mathf.MoveTowards(currentWind, targetWind, windChangeRate * Time.deltaTime);

        // Change the wind speed when the target wind is reached.
        if (Mathf.Approximately(currentWind, targetWind))
        {
            targetWind = Random.Range(windMin, windMax);
        }
    }

    /// <summary>
    /// Set the rain intensity.
    /// </summary>
    void UpdateRain()
    {
        float rainDelta = Mathf.Abs(targetRain - currentRain);
        float rainChangeRate = (rainDelta / rainTransitionDuration);
        currentRain = Mathf.MoveTowards(currentRain, targetRain, rainChangeRate * Time.deltaTime);

        // Change the rain intensity when the target rain is reached.
        if (Mathf.Approximately(currentRain, targetRain))
        {
            targetRain = Random.Range(rainMin, rainMax);
        }

        // Update rain audio volume based on currentRain intensity.
        if (rainAudioSource != null)
        {
            float normalizedRain = Mathf.InverseLerp(rainMin, rainMax, currentRain);
            rainAudioSource.volume = Mathf.Lerp(minRainVolume, maxRainVolume, normalizedRain);
        }
    }

    /// <summary>
    /// A coroutine to spawn lightning at random intervals.
    /// </summary>
    IEnumerator LightningRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minLightningInterval, maxLightningInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnLightning();
        }
    }

    /// <summary>
    /// Spawn lightning in front of the player, at a certain distance.
    /// </summary>
    void SpawnLightning()
    {
        if (player == null || lightningPrefab == null) return;

        Vector3 forward = player.forward;
        Vector3 spawnPosition = player.position + forward * lightningDistance;
        spawnPosition += new Vector3(Random.Range(-lightningOffset.x, lightningOffset.x), 0,
                                     Random.Range(-lightningOffset.y, lightningOffset.y));

        // Instantiate the lightning effect.
        var tempLighting = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);

        // Play thunder sound.
        if (audioSource != null && thunderSound != null)
        {
            audioSource.PlayOneShot(thunderSound);
        }

        // Destroy the lightning effect after 0.5 second.
        Destroy(tempLighting, 0.5f);
    }

    /// <summary>
    /// Set the indoor state of the player.
    /// </summary>
    /// <param name="indoor"> Defines if the player is indoors or not. </param>
    public void SetIndoor(bool indoor)
    {
        isIndoor = indoor;

        if (isIndoor)
        {
            // Stop or mute weather effects when indoors.

            if(lightningPrefab != null)
            {
                audioSource.Stop();

                // Stop the lightning effect.
                StopCoroutine(LightningRoutine());
            }

            if (rainEffect != null)
            {
                // Stop the rain effect.
                VisualEffect rainVFX = gameObject.GetComponentInChildren<VisualEffect>();

                if (rainVFX != null)
                {
                    rainVFX.Stop();
                }
            }

            if (rainAudioSource != null)
            {
                rainAudioSource.volume = 0f;
            }
        }
        else
        {
            if(lightningPrefab != null) 
            {
                audioSource.Play();

                // Resume lightning effects when outdoors.
                StartCoroutine(LightningRoutine());
            }

            if (rainEffect != null)
            {
                // Start the rain effect.
                VisualEffect rainVFX = gameObject.GetComponentInChildren<VisualEffect>();

                if (rainVFX != null)
                {
                    rainVFX.Play();
                }
            }

            // Resume weather effects when outdoors.
            if (rainAudioSource != null)
            {
                // Reset the rain audio volume based on the current rain intensity.
                float normalizedRain = Mathf.InverseLerp(rainMin, rainMax, currentRain);
                rainAudioSource.volume = Mathf.Lerp(minRainVolume, maxRainVolume, normalizedRain);
            }
        }
    }

    #endregion
}
