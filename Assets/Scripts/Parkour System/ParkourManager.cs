using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the parkour actions and the stamina system.
/// It checks for input and conditions to start parkour actions.
/// It handles the stamina deduction for each action.
/// and also manages the stamina regeneration over time.
/// It implements the <see cref="IParkourAction"/> interface.
/// </summary>
public class ParkourManager : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Reference to the CharacterController component.
    /// </summary>
    protected CharacterController controller;

    /// <summary>
    /// Reference to the player camera.
    /// </summary>
    protected Transform playerCam;

    /// <summary>
    /// List of parkour actions available to the player.
    /// </summary>
    protected List<IParkourAction> parkourActions;

    /// <summary>
    /// The current parkour action being executed.
    /// </summary>
    protected IParkourAction currentAction;

    /// <summary>
    /// Reference to the stamina meter component.
    /// </summary>
    protected StaminaMeter staminaMeter;

    /// <summary>
    /// Flag to determine if stamina is used for parkour actions.
    /// </summary>
    [HideInInspector]
    public bool useStamina = true;

    #endregion

    protected virtual void Awake()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (playerCam == null)
        {
            playerCam = Camera.main.transform;
        }

        if (staminaMeter == null)
        {
            staminaMeter = GetComponent<StaminaMeter>();
        }

        parkourActions = new List<IParkourAction>(GetComponents<IParkourAction>());

        foreach (IParkourAction action in parkourActions)
        {
            action.Initialize(controller, playerCam);
        }
    }

    protected virtual void Update()
    {
        // If there is a current action
        if (currentAction != null)
        {
            // Execute the action
            currentAction.Execute();

            // If the action is no longer active
            if (!currentAction.IsActive)
            {
                // Exit the action
                currentAction.Exit();
                currentAction = null;
            }
        }
        else
        {
            // Check for input and conditions to start a parkour action
            foreach (IParkourAction action in parkourActions)
            {
                // Check if the action is active
                action.CheckInput();

                // If the action is active
                if (action.IsActive)
                {
                    // If stamina is not used, or if used and enough stamina is available
                    if (!useStamina || (staminaMeter != null && staminaMeter.CanPerformAction(action.StaminaCost)))
                    {
                        // Deduct stamina if used
                        if (useStamina && staminaMeter != null)
                        {
                            // Reduce stamina
                            staminaMeter.ReduceStamina(action.StaminaCost);
                        }
                        currentAction = action;
                        currentAction.Enter();
                        break;
                    }
                }
            }
        }
    }
}