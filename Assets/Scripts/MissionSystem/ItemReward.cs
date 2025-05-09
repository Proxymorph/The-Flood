using UnityEngine;

/// <summary>
/// Reward that unlocks an item. It is a ScriptableObject that implements the <see cref="BaseReward"/> abstract.
/// </summary>

[CreateAssetMenu(fileName = "NewItemReward", menuName = "Missions/Rewards/Item Reward")]
public class ItemReward : BaseReward
{
    [Header("Item Reward Settings")]

    /// <summary>
    /// The reward item that will be unlocked.
    /// </summary>
    public ScriptableObject rewardItem;

    /// <summary>
    /// Called to apply this reward to the player.
    /// </summary>
    public override void ApplyReward()
    {
        // Apply the reward here.
        Debug.Log("Reward Applied: " + rewardName + " - Unlocking: " + (rewardItem != null ? rewardItem.name : "None"));
    }
}