using TMPro;
using UnityEngine;

/// <summary>
/// Manages the interaction between the player and interactable objects.
/// It casts a ray from the center of the screen to detect interactable objects.
/// And manages the UI prompt appearing on screen and interaction key press.
/// </summary>
public class InteractableManager : MonoBehaviour
{
    #region Properties

    [Header("Interaction Settings")]

    /// <summary>
    /// Reference to the player's camera.
    /// </summary>
    [Tooltip("Reference to the player's camera.")]
    public Camera playerCamera;

    /// <summary>
    /// Reference to the interaction prompt text.
    /// </summary>
    private TMP_Text interactionPrompt;

    /// <summary>
    /// Maximum distance for interaction.
    /// </summary>
    [Tooltip("Maximum distance for interaction.")]
    public float interactionDistance = 3f;

    /// <summary>
    /// Key used to interact.
    /// </summary>
    [Tooltip("Key used to interact.")]
    public KeyCode interactionKey = KeyCode.F;

    /// <summary>
    /// The current interactable object detected.
    /// </summary>
    private IInteractable currentInteractable;

    #endregion

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Find the interaction prompt text component.
        if (interactionPrompt == null)
        {
            interactionPrompt = GameObject.Find("Interactable_Message").GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        CheckForInteractable();
        HandleInput();
    }

    #region Utility Methods

    /// <summary>
    /// Casts a ray from the center of the screen to check for interactable objects.
    /// </summary>
    void CheckForInteractable()
    {
        // Cast a ray from the center of the screen.
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            // Look for a component implementing IInteractable on the hit object.
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            // If an interactable object is found, display its interaction prompt.
            if (interactable != null)
            {
                currentInteractable = interactable;
                interactionPrompt.text = currentInteractable.GetInteractionPrompt();
                return;
            }
        }
        else // If no interactable object is found, clear the interaction prompt.
        {
            interactionPrompt.text = string.Empty;
        }

        // Reset the current interactable object.
        currentInteractable = null;
    }

    /// <summary>
    /// Checks for the interaction key press and, if an interactable is detected, calls its Interact() method.
    /// </summary>
    void HandleInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            // Call the Interact() method on the current interactable object.
            currentInteractable.Interact();
        }
    }

    #endregion
}