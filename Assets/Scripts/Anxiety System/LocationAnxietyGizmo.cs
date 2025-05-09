using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A gizmo that visualizes the location of an anxiety factor in the Scene view.
/// </summary>
public class LocationAnxietyGizmo : MonoBehaviour
{
    #region Properties

    [Header("Gizmo Settings")]
    /// <summary>
    /// Manual override detection radius (used if no LocationBasedAnxietyFactor is found).
    /// </summary>
    [Tooltip("Manual override detection radius (used if no LocationBasedAnxietyFactor is found).")]
    public float detectionRadius = 10f;

    /// <summary>
    /// Color of the gizmo in the Scene view.
    /// </summary>
    [Tooltip("Color of the gizmo in the Scene view.")]
    public Color gizmoColor = Color.blue;

    /// <summary>
    /// Optional label to display the location name.
    /// </summary>
    [Tooltip("Optional label to display the location name.")]
    public string locationLabel = "Anxiety Location";

    #endregion

    private void OnDrawGizmos()
    {
        // Try to get a LocationBasedAnxietyFactor component from this GameObject.
        float radius = detectionRadius;
        LocationBasedAnxietyFactor anxietyFactor = GetComponent<LocationBasedAnxietyFactor>();
        if (anxietyFactor != null)
        {
            radius = anxietyFactor.maxDistance;
        }

        // Set the gizmo color and draw the sphere.
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);

#if UNITY_EDITOR
        // Draw the label above the sphere.
        Handles.Label(transform.position + Vector3.up * (radius + 0.5f), locationLabel);
#endif
    }

    // This ensures that changes in the Inspector are immediately reflected in the Scene view.
    private void OnValidate()
    {
#if UNITY_EDITOR
        SceneView.RepaintAll();
#endif
    }
}