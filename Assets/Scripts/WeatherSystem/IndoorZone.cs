using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Simple script to detect when the player enters or exits an indoor zone.
/// </summary>
public class IndoorZone : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Optional label to display the location name.
    /// </summary>
    [Tooltip("Optional label to display the location name.")]
    public string locationLabel = "Indoor Zone";

    /// <summary>
    /// Tag of the player GameObject.
    /// </summary>
    [Tooltip("Tag of the player GameObject.")]
    public string playerTag = "Player";

    /// <summary>
    /// Reference to the WeatherSystem component.
    /// </summary>
    [Tooltip("Reference to the WeatherSystem component.")]
    public WeatherSystem weatherSystem;

    /// <summary>
    /// Collider component used to detect the player entering or exiting the zone.
    /// </summary>
    private Collider zoneCollider;

    #endregion

    private void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        if (zoneCollider == null)
        {
            Debug.LogWarning("IndoorZone requires a Collider component (set as trigger).");
        }

        // If no WeatherSystem was assigned, try to find it in the scene.
        if (weatherSystem == null)
        {
            weatherSystem = FindFirstObjectByType<WeatherSystem>();
            if (weatherSystem == null)
            {
                Debug.LogWarning("IndoorZone: No WeatherSystem found in the scene.");
            }
        }
    }

    #region Collider Utility

    /// <summary>
    /// Find the first object in the scene with the specified type.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the zone.
        if (other.CompareTag(playerTag))
        {
            if (weatherSystem != null)
            {
                // Disable weather effects when the player enters the indoor zone.
                weatherSystem.SetIndoor(true);
                Debug.Log("Player entered indoor zone: weather effects disabled.");
            }
        }
    }

    /// <summary>
    /// Find the first object in the scene with the specified type.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // Check if the player entered the zone.
        if (other.CompareTag(playerTag))
        {
            if (weatherSystem != null && weatherSystem.isIndoor == false)
            {
                // Disable weather effects when the player enters the indoor zone.
                weatherSystem.SetIndoor(true);
                Debug.Log("Player entered indoor zone: weather effects disabled.");
            }
        }
    }

    /// <summary>
    /// Find the first object in the scene with the specified type.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the zone.
        if (other.CompareTag(playerTag))
        {
            if (weatherSystem != null)
            {
                // Enable weather effects when the player exits the indoor zone.
                weatherSystem.SetIndoor(false);
                Debug.Log("Player exited indoor zone: weather effects enabled.");
            }
        }
    }

    #endregion

    #region Gizmos

#if UNITY_EDITOR
    /// <summary>
    /// Draw the collider bounds in the Scene view.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Draw a wireframe box based on the collider's bounds.
        Collider col = zoneCollider != null ? zoneCollider : GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            // Also display the radius if it's a SphereCollider.
            if (col is SphereCollider sphere)
            {
                Gizmos.DrawWireSphere(transform.position + sphere.center, sphere.radius);

                // Draw the label above the sphere.
                Handles.Label(transform.position + Vector3.up * (sphere.radius + 0.5f), locationLabel);
            }
        }
    }

    /// <summary>
    /// Find the first object in the scene with the specified type.
    /// </summary>
    private void OnValidate()
    {
        // Repaint the Scene view when properties change.
        SceneView.RepaintAll();
    }
#endif

    #endregion
}