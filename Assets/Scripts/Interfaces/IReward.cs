/// <summary>
/// Interface for rewards that can be applied to the player.
/// Used to create various types of rewards, like <see cref="ItemReward"/>.
/// </summary>
public interface IReward
{
    /// <summary>
    /// The display name of this reward.
    /// </summary>
    string RewardName { get; }

    /// <summary>
    /// Called to apply this reward to the player with a custom logic.
    /// </summary>
    void ApplyReward();
}