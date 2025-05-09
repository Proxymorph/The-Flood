using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A custom condition that checks if the player has a certain number of pills in their inventory.
/// </summary>
[CreateAssetMenu(fileName = "NewPillBottleCondition", menuName = "Missions/Conditions/Pill Bottle Condition")]
public class PillBottleCondition : BaseMissionCondition
{
    [Header("Pill Bottle Condition Settings")]

    /// <summary>
    /// The required number of pills the player must have.
    /// </summary>
    [Tooltip("The required number of pills the player must have.")]
    public int requiredPillCount = 1;

    /// <summary>
    /// A method that checks if the player has the required number of pills.
    /// </summary>
    /// <returns> True or False based on how many pills are in the inventory and the required pill count. </returns>
    public override bool IsConditionMet()
    {
        return (PillInventory.Instance != null) && (PillInventory.Instance.pillCount >= requiredPillCount);
    }
}