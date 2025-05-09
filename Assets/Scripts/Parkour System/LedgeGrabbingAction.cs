using UnityEngine;
using System.Collections;

/// <summary>
/// Allows the player to grab ledges and climb up.
/// It detects ledges in front of the player and checks if there is ground below.
/// And it implements the <see cref="IParkourAction"/> interface.
/// </summary>
public class LedgeGrabbingAction : MonoBehaviour, IParkourAction
{
    #region Properties

    /// <summary>
    /// Key used to interact.
    /// </summary>
    [Tooltip("Key used to interact.")]
    public KeyCode interactionKey = KeyCode.E;

    public CharacterController controller;
    public Transform playerCam;

    public float ledgeGrabDistance = 1.2f;
    public float climbSpeed = 4f;
    public LayerMask wallMask;

    private bool isActive = false;
    private RaycastHit hit;

    public bool IsActive { get { return isActive; } }

    public float StaminaCost { get { return 15f; } }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Initialize the action with required references.
    /// </summary>
    /// <param name="controller"> The player character controller. </param>
    /// <param name="playerCam"> The camera that handles the player view. </param>
    public void Initialize(CharacterController controller, Transform playerCam)
    {
        this.controller = controller;
        this.playerCam = playerCam;
    }

    /// <summary>
    /// Check for input to trigger the ledge grab action.
    /// </summary>
    public void CheckInput()
    {
        // If "interactionKey" is held and a ledge is detected ahead
        if (Input.GetKey(interactionKey))
        {
            Vector3 origin = transform.position + Vector3.up * 1.5f;
            if (Physics.Raycast(origin, transform.forward, out hit, ledgeGrabDistance, wallMask))
            {
                // Check if there is ground below the ledge
                if (!Physics.Raycast(hit.point + Vector3.up * 0.5f, Vector3.down, 1f, wallMask))
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
        StartCoroutine(ClimbLedge());
    }

    /// <summary>
    /// Coroutine to climb the ledge.
    /// It moves the player from the current position to the ledge position.
    /// </summary>
    private IEnumerator ClimbLedge()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = hit.point + Vector3.up * 1.5f;
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            controller.enabled = false;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime * climbSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        controller.enabled = true;
        isActive = false;
    }

    public void Execute()
    {
        // Ledge climbing is executed via coroutine.
    }

    public void Exit()
    {
        // Reset or cleanup if needed.
    }

    #endregion

}