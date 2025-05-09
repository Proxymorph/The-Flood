using UnityEngine;
/// <summary>
/// An interface for afflictions that can be applied to the player.
/// Defines the basic properties and methods for an affliction.
/// </summary>
public interface IAffliction
{
    /// <summary>
    /// The display name of the affliction.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The icon to show on screen. Managed by the <see cref="AfflictionUIManager"/>.
    /// </summary>
    Sprite Icon { get; }

    /// <summary>
    /// How long the affliction will affect the player (in seconds).
    /// This property is modifiable so it can be decremented over time.
    /// The duration will be displayed on screen.
    /// When reaching 0, the affliction will be removed.
    /// </summary>
    float Duration { get; set; }

    /// <summary>
    /// Applies the affliction to the player.
    /// </summary>
    void Apply();

    /// <summary>
    /// Removes the affliction from the player.
    /// </summary>
    void Remove();
}