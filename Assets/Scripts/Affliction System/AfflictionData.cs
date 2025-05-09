using UnityEngine;

[CreateAssetMenu(fileName = "NewAffliction", menuName = "Afflictions/Affliction Data")]

/// <summary>
/// This class holds the data for an affliction in the form of a <see cref="ScriptableObject"/>.
/// </summary>
public class AfflictionData : ScriptableObject
{
    /// <summary>
    /// The name of the affliction.
    /// </summary>
    public string afflictionName;

    /// <summary>
    /// The icon to display for the affliction in the UI.
    /// Used togheter with <see cref="AfflictionUIManager"/>.
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// The base duration of the affliction.
    /// This is expressed in seconds.
    /// </summary>
    public float baseDuration = 5f;
}