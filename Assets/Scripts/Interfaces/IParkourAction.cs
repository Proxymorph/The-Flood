using UnityEngine;

/// <summary>
/// Create a blueprint for how a parkour action would function.
/// It takes in account stamina.
/// It has 3 main section: Enter, Execute and Exit.
/// </summary>
public interface IParkourAction
{
    /// <summary>
    /// Whether the action is currently active.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// The stamina cost for this action (e.g., in absolute stamina points).
    /// </summary>
    float StaminaCost { get; }

    /// <summary>
    /// Initialize the action with required references.
    /// </summary>
    /// <param name="controller"> The player controller. </param>
    /// <param name="playerCam"> The player camera. </param>
    void Initialize(CharacterController controller, Transform playerCam);

    /// <summary>
    /// Check if input and conditions allow this action to start.
    /// </summary>
    void CheckInput();

    /// <summary>
    /// Called when the action starts.
    /// </summary>
    void Enter();

    /// <summary>
    /// Called every frame while the action is active.
    /// </summary>
    void Execute();

    /// <summary>
    /// Called when the action ends.
    /// </summary>
    void Exit();
}