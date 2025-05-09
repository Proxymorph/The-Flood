using UnityEngine;

/// <summary>
/// Base class for all rewards. Derive from this class to create new rewards (just like: <see cref="ItemReward"/>)."/>
/// It implements the <see cref="IReward"/> interface to define the reward name and how the reward is applied.
/// </summary>
public abstract class BaseReward : ScriptableObject, IReward
{
    [Header("Reward Basic Info")]

    /// <summary>
    /// The display name of this reward.
    /// </summary>
    public string rewardName;

    /// <summary>
    /// The display name of this reward.
    /// </summary>
    public string RewardName => rewardName;

    /// <summary>
    /// Called to apply this reward to the player.
    /// Ovveride this method to apply the reward to the player.
    /// </summary>
    public abstract void ApplyReward();
}