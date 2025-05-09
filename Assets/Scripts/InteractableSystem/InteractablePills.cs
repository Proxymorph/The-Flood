using UnityEngine;

/// <summary>
/// An interactable object that gives the player the ability to collect pills.
/// It implements the <see cref="IInteractable"/> interface.
/// </summary>
public class InteractablePills : MonoBehaviour, IInteractable
{
    #region Properties

    [Header("Pill Settings")]

    ///<summary>
    /// The number of pills to add to the player's inventory.
    ///</summary>
    [Tooltip("The number of pills to add to the player's inventory.")]
    public int pillCount = 1;

    /// <summary>
    /// The display name for this pill item.
    /// </summary>
    [Tooltip("The display name for this pill item.")]
    public string pillName = "Anti-Anxiety Pills";

    /// <summary>
    /// The icon for the UI prompt (optional).
    /// </summary>
    [Tooltip("The icon for the UI prompt (optional).")]
    public Sprite icon;

    #endregion

    #region IInteractable Implementation

    /// <summary>
    /// Called when the player interacts with the object.
    /// </summary>
    public void Interact()
    {
        // Add the pill count to the player's inventory.
        if (PillInventory.Instance != null)
        {
            PillInventory.Instance.AddPills(pillCount);
        }
        else
        {
            Debug.LogWarning("PillInventory instance not found!");
        }

        // Remove the pill object from the scene.
        Destroy(gameObject);
    }

    /// <summary>
    /// Returns an optional prompt on screen.
    /// </summary>
    /// <returns> Interaction guidance text. </returns>
    public string GetInteractionPrompt()
    {
        return $"Press F to collect {pillCount} {pillName}";
    }

    #endregion
}