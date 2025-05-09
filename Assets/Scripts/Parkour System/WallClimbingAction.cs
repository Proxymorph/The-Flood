using UnityEngine;
using System.Collections;

/// <summary>
/// Wall climb action for the parkour system.
/// It allows the player to climb a wall for a limited time.
/// And it has a stamina cost.
/// It implements the <see cref="IParkourAction"/> interface.
/// </summary>
public class WallClimbAction : MonoBehaviour, IParkourAction
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
    /// The speed at which the player climbs the wall.
    /// </summary>
    [Tooltip("The speed at which the player climbs the wall.")]
    public float wallClimbSpeed = 4f;

    /// <summary>
    /// The maximum time the player can climb the wall.
    /// </summary>
    [Tooltip("The maximum time the player can climb the wall.")]
    public float maxWallClimbTime = 1.5f;

    /// <summary>
    /// The distance to check for a wall in front of the player.
    /// </summary>
    [Tooltip("The distance to check for a wall in front of the player.")]
    public float wallCheckDistance = 1f;

    /// <summary>
    /// The layer mask for the wall.
    /// </summary>
    [Tooltip("The layer mask for the wall.")]
    public LayerMask wallMask;

    /// <summary>
    /// Whether the action is currently active.
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// The timer for the wall climb.
    /// </summary>
    private float wallClimbTimer;

    /// <summary>
    /// The hit information for the wall.
    /// </summary>
    private RaycastHit hit;

    /// <summary>
    /// Whether the action is currently active.
    /// </summary>
    public bool IsActive { get { return isActive; } }

    /// <summary>
    /// The stamina cost for this action.
    /// </summary>
    public float StaminaCost { get { return 12f; } }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Initialize the action with required references.
    /// </summary>
    /// <param name="controller"> The controller of the player. </param>
    /// <param name="playerCam"> The camera that controls player vision. </param>
    public void Initialize(CharacterController controller, Transform playerCam)
    {
        this.controller = controller;
        this.playerCam = playerCam;
    }

    /// <summary>
    /// Check if input and conditions allow this action to start.
    /// </summary>
    public void CheckInput()
    {
        // Initiate wall climb when W is pressed and a wall is detected.
        if (Input.GetKey(KeyCode.W))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, wallMask))
            {
                isActive = true;
            }
        }
    }

    #endregion

    #region IParkourAction Implementation

    /// <summary>
    /// Called when the action starts.
    /// </summary>
    public void Enter()
    {
        wallClimbTimer = maxWallClimbTime;
    }


    /// <summary>
    /// Called every frame while the action is active.
    /// </summary>
    public void Execute()
    {
        if (wallClimbTimer > 0 && Input.GetKey(KeyCode.W))
        {
            wallClimbTimer -= Time.deltaTime;
            controller.Move(Vector3.up * wallClimbSpeed * Time.deltaTime);
        }
        else
        {
            isActive = false;
        }
    }

    /// <summary>
    /// Called when the action ends.
    /// </summary>
    public void Exit()
    {
        // Wall climb is handled in Execute().
    }

    #endregion
}