using UnityEngine;

/// <summary>
/// Interface for creating interactable objects.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called when the player interacts with the object.
    /// </summary>
    void Interact();

    /// <summary>
    /// Returns an optional prompt on screen.
    /// </summary>
    string GetInteractionPrompt();
}