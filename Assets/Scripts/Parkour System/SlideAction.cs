using UnityEngine;
using System.Collections;

/// <summary>
/// Slide action for the parkour system.
/// It allows the player to slide on the ground.
/// And it has a stamina cost.
/// It implements the <see cref="IParkourAction"/> interface.
/// </summary>
public class SlideAction : MonoBehaviour, IParkourAction
{
    #region Properties

    public CharacterController controller;
    public Transform playerCam;

    public float slideSpeed = 10f;
    public float slideDuration = 0.8f;
    public float slideHeight = 0.5f;

    private bool isActive = false;
    private float originalHeight;

    public bool IsActive { get { return isActive; } }

    public float StaminaCost { get { return 8f; } }

    #endregion

    void Start()
    {
        originalHeight = controller.height;
    }

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

    public void CheckInput()
    {
        // Trigger slide when Left Control is pressed and the player is grounded.
        if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded)
        {
            isActive = true;
        }
    }

    #endregion


    #region IParkourAction Implementation

    public void Enter()
    {
        StartCoroutine(Slide());
    }

    /// <summary>
    /// Coroutine that handles the sliding action.
    /// It moves the player forward at a fixed speed for a fixed duration.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Slide()
    {
        controller.height = slideHeight;
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            // Keep sliding forward on flat ground.
            Vector3 slideDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            controller.Move(slideDirection * slideSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        controller.height = originalHeight;
        isActive = false;
    }

    public void Execute()
    {
        // Slide is managed by the coroutine.
    }

    public void Exit()
    {
        // Nothing to do here.
    }

    #endregion
}