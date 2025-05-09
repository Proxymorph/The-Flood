using UnityEngine;

/// <summary>
/// An interactable door that can be opened and closed.
/// </summary>
public class InteractableDoor : MonoBehaviour, IInteractable
{
    #region Properties

    /// <summary>
    /// Whether the door is open or closed.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Whether the door is locked.
    /// </summary>
    [SerializeField]
    private bool isLocked = false;

    /// <summary>
    /// Encapsulated whether the door is open or closed.
    /// To be used outside of the class.
    /// </summary>
    public bool IsLocked { get => isLocked; set => isLocked = value; }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Unlocks the door. This can be called by a mission complete event.
    /// </summary>
    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("Door unlocked!");
    }

    #endregion

    #region IInteractable Implementation

    /// <summary>
    /// Called when the player interacts with the door.
    /// </summary>
    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("Door is locked!");
            return;
        }

        // Toggle door state.
        isOpen = !isOpen;
        if (isOpen)
        {
            Debug.Log("Door opened!");
            gameObject.transform.Rotate(0, 90, 0);
        }
        else
        {
            Debug.Log("Door closed!");
            gameObject.transform.Rotate(0, -90, 0);
        }
    }

    /// <summary>
    /// Returns a prompt to display on screen.
    /// </summary>
    /// <returns></returns>
    public string GetInteractionPrompt()
    {

        if(isLocked)
        {
            return "Door is locked";
        }
        else
        {
            return isOpen ? "Press F to close door" : "Press F to open door";
        }
    }

    #endregion
}