using UnityEngine;
using System.Collections;

/// <summary>
/// A vault action that allows the player to vault over obstacles.
/// It is a parkour action that implements the <see cref="IParkourAction"/> interface.
/// And uses a coroutine to handle the vaulting animation.
/// </summary>
public class VaultAction : MonoBehaviour, IParkourAction
{
    #region Properties

    /// <summary>
    /// Key used to interact.
    /// </summary>
    [Tooltip("Key used to interact.")]
    public KeyCode interactionKey = KeyCode.E;

    /// <summary>
    /// The player controller.
    /// </summary>
    [Tooltip("The player controller.")]
    public CharacterController controller;

    /// <summary>
    /// The player camera.
    /// </summary>
    [Tooltip("The player camera.")]
    private Transform playerCam;

    /// <summary>
    /// The height of the vaultable obstacle.
    /// </summary>
    [Tooltip("The height of the vaultable obstacle.")]
    public float vaultHeight = 1.5f;

    /// <summary>
    /// The speed of the vaulting animation.
    /// </summary>
    [Tooltip("The speed of the vaulting animation.")]
    public float vaultSpeed = 6f;

    /// <summary>
    /// The layer mask for vaultable obstacles.
    /// </summary>
    [Tooltip("The layer mask for vaultable obstacles.")]
    public LayerMask vaultMask;

    /// <summary>
    /// Whether the action is currently active.
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// The hit information from the raycast.
    /// </summary>
    private RaycastHit hit;

    /// <summary>
    /// Whether the action is currently active.
    /// </summary>
    public bool IsActive { get { return isActive; } }

    /// <summary>
    /// The stamina cost for this action (e.g., in absolute stamina points).
    /// </summary>
    public float StaminaCost { get { return 10f; } }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Initialize the action with required references.
    /// </summary>
    /// <param name="controller"> The player controller. </param>
    /// <param name="playerCam"> The camera that handles the player view. </param>
    public void Initialize(CharacterController controller, Transform playerCam)
    {
        this.controller = controller;
        this.playerCam = playerCam;
    }

    /// <summary>
    /// Check for input to initiate the vault action.
    /// </summary>
    public void CheckInput()
    {
        // Trigger vault when the "interactionKey" is pressed and a vaultable obstacle is detected.
        if (Input.GetKeyDown(interactionKey))
        {
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(origin, transform.forward, out hit, 1f, vaultMask))
            {
                if (hit.collider.bounds.size.y <= vaultHeight)
                {
                    isActive = true;
                }
            }
        }
    }

    #endregion

    #region IParkourAction Implementation

    public void Enter()
    {
        StartCoroutine(VaultOver());
    }

    /// <summary>
    /// Coroutine to handle the vaulting animation.
    /// It moves the player from the starting position to the end position over time.
    /// </summary>
    private IEnumerator VaultOver()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = hit.point + Vector3.up * 1.2f;
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            controller.enabled = false;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime * vaultSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        controller.enabled = true;
        isActive = false;
    }

    public void Execute()
    {
        // Vault is handled via coroutine.
    }

    public void Exit()
    {
        // No exit logic required.
    }

    #endregion
}