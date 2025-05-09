using UnityEngine;

/// <summary>
/// A custom mission condition that checks if the player has reached a specific area.
/// </summary>

[CreateAssetMenu(fileName = "NewReachAreaCondition", menuName = "Missions/Conditions/Reach Area Condition")]
public class ReachAreaCondition : BaseMissionCondition
{
    [Header("Reach Area Settings")]

    /// <summary>
    /// Optional: If assigned, the condition will use this object's position as the target area.
    /// </summary>
    [Tooltip("Optional: If assigned, the condition will use this object's position as the target area.")]
    public Transform targetTransform;

    /// <summary>
    /// If no target transform is assigned, this target position will be used.
    /// </summary>
    [Tooltip("If no target transform is assigned, this target position will be used.")]
    public Vector3 targetPosition;

    /// <summary>
    /// Distance from the target (or target transform) required to fulfill the condition.
    /// </summary>
    [Tooltip("Distance from the target (or target transform) required to fulfill the condition.")]
    public float radius = 5f;

    public override bool IsConditionMet()
    {
        Vector3 targetPos = (targetTransform != null) ? targetTransform.position : targetPosition;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogWarning("ReachAreaCondition: No player found with tag 'Player'.");
            return false;
        }

        float distance = Vector3.Distance(playerObj.transform.position, targetPos);
        Debug.Log("ReachAreaCondition: Distance to target = " + distance + ", Radius = " + radius);

        return distance <= radius;
    }
}