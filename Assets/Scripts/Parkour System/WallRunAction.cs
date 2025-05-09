using UnityEngine;

/// <summary>
/// Wall run action that allows the player to run on walls.
/// It has a timer to limit the duration of the wall run.
/// And a wall jump mechanic to jump off the wall.
/// It implements the <see cref="IParkourAction"/> interface.
/// </summary>
public class WallRunAction : MonoBehaviour, IParkourAction
{
    #region Properties

    /// <summary>
    /// The player controller.
    /// </summary>
    [Tooltip("The player controller.")]
    public CharacterController controller;

    /// <summary>
    /// The player camera.
    /// </summary>
    [Tooltip("The player camera.")]
    public Transform playerCam;

    /// <summary>
    /// The speed at which the player moves while wall running.
    /// </summary>
    [Tooltip("The speed at which the player moves while wall running.")]
    public float wallRunSpeed = 8f;

    /// <summary>
    /// The gravity applied while wall running.
    /// </summary>
    [Tooltip("The gravity applied while wall running.")]
    public float wallRunGravity = 1f;

    /// <summary>
    /// The force applied when jumping off a wall.
    /// </summary>
    [Tooltip("The force applied when jumping off a wall.")]
    public float wallJumpForce = 12f;

    /// <summary>
    /// The maximum time the player can wall run.
    /// </summary>
    [Tooltip("The maximum time the player can wall run.")]
    public float maxWallRunTime = 2f;

    /// <summary>
    /// The distance to check for walls.
    /// </summary>
    [Tooltip("The distance to check for walls.")]
    public float wallCheckDistance = 1f;

    /// <summary>
    /// The layer mask to filter the wall detection.
    /// </summary>
    [Tooltip("The layer mask to filter the wall detection.")]
    public LayerMask wallMask;

    /// <summary>
    /// The timer to limit the duration of the wall run.
    /// </summary>
    private float wallRunTimer;

    /// <summary>
    /// A flag to check if the wall run is active.
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// The hit information of the wall.
    /// </summary>
    private RaycastHit wallHit;

    /// <summary>
    /// The wall run action is active.
    /// </summary>
    [Tooltip("The wall run action is active.")]
    public bool IsActive { get { return isActive; } }

    /// <summary>
    /// The stamina cost of the wall run action.
    /// </summary>
    [Tooltip("The stamina cost of the wall run action.")]
    public float StaminaCost { get { return 5f; } }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Checks for input to activate the wall run.
    /// </summary>
    /// <param name="controller"> The controller of the player. </param>
    /// <param name="playerCam"> The camera that controls player vision. </param>
    public void Initialize(CharacterController controller, Transform playerCam)
    {
        this.controller = controller;
        this.playerCam = playerCam;
    }

    /// <summary>
    /// Checks for input to activate the wall run.
    /// </summary>
    public void CheckInput()
    {
        // Initiate wall run when Space is pressed and the player is not grounded.
        if (Input.GetKey(KeyCode.Space) && !controller.isGrounded)
        {
            Vector3 origin = transform.position + Vector3.up;
            if (Physics.Raycast(origin, -playerCam.right, out wallHit, wallCheckDistance, wallMask) ||
                Physics.Raycast(origin, playerCam.right, out wallHit, wallCheckDistance, wallMask))
            {
                isActive = true;
            }
        }
    }

    #endregion

    #region IParkourAction Implementation

    /// <summary>
    /// Enters the wall run action.
    /// </summary>
    public void Enter()
    {
        // Initialize the wall run timer when entering the action.
        wallRunTimer = maxWallRunTime;
    }

    /// <summary>
    /// Executes the wall run action.
    /// </summary>
    public void Execute()
    {
        if (wallRunTimer <= 0)
        {
            isActive = false;
            return;
        }
        wallRunTimer -= Time.deltaTime;

        Vector3 forwardMovement = transform.forward * wallRunSpeed;
        Vector3 move = new Vector3(forwardMovement.x, -wallRunGravity, forwardMovement.z);
        controller.Move(move * Time.deltaTime);

        // Example: Pressing Space during wall run might trigger a wall jump.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 wallJumpDir = transform.up + wallHit.normal * -1f;
            controller.Move(wallJumpDir * wallJumpForce * Time.deltaTime);
            isActive = false;
        }
    }

    /// <summary>
    /// Exits the wall run action.
    /// </summary>
    public void Exit()
    {
        // Nothing to do here.
    }

    #endregion
}
