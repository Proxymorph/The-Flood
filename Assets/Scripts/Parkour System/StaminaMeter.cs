using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player's stamina.
/// It handles the deduction of stamina based on the player's actions.
/// And regenerates stamina over time.
/// </summary>
public class StaminaMeter : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Reference to the CharacterController component.
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// Reference to the stamina slider UI element.
    /// </summary>
    private Slider staminaSlider;

    [Header("Stamina Settings")]

    /// <summary>
    /// Maximum stamina value.
    /// </summary>
    [Tooltip("Maximum stamina value.")]
    public float maxStamina = 100f;

    /// <summary>
    /// Stamina regeneration rate per second.
    /// </summary>
    [Tooltip("Stamina regeneration rate per second.")]
    public float regenRate = 10f;

    [Header("Stamina Costs")]

    /// <summary>
    /// Stamina cost for sprinting.
    /// </summary>
    [Tooltip("Stamina cost for sprinting.")]
    public float sprintCost = 0.1f;

    /// <summary>
    /// Stamina cost for jumping.
    /// </summary>
    [Tooltip("Stamina cost for jumping.")]
    public float jumpCost = 5f;

    /// <summary>
    /// Flag to prevent multiple stamina deductions for a single jump.
    /// </summary>
    private bool jumpDeducted = false;

    /// <summary>
    /// Current stamina value.
    /// </summary>
    [HideInInspector] 
    public float currentStamina;

    #endregion

    void Awake()
    {
        if(characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (staminaSlider == null)
        {
            staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
        }

        currentStamina = maxStamina;
    }

    void Update()
    {
        staminaSlider.value = currentStamina / maxStamina;

        if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }

        ReduceStaminaOnSprint();

        ReduceStaminaOnJump();
    }

    #region Utility Methods

    /// <summary>
    /// Checks if the player has enough stamina to perform an action.
    /// </summary>
    /// <param name="cost"> The cost of an action. </param>
    /// <returns></returns>
    public bool CanPerformAction(float cost)
    {
        return currentStamina >= cost;
    }

    /// <summary>
    /// Reduces the player's stamina by a specified amount.
    /// </summary>
    /// <param name="cost"> The cost of an action. </param>
    public void ReduceStamina(float cost)
    {
        currentStamina -= cost;
        if (currentStamina < 0f)
        {
            currentStamina = 0f;
        }
    }

    /// <summary>
    /// Reduces the player's stamina when sprinting.
    /// </summary>
    public void ReduceStaminaOnSprint()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 3f)
        {
            ReduceStamina(sprintCost);
        }
    }

    /// <summary>
    /// Reduces the player's stamina when jumping.
    /// </summary>
    public void ReduceStaminaOnJump()
    {
        // Only deduct stamina when jump is first pressed while grounded.
        if (Input.GetButtonDown("Jump") && !characterController.isGrounded && !jumpDeducted)
        {
            ReduceStamina(jumpCost);
            jumpDeducted = true;
        }

        // Reset the flag once the character is grounded.
        if (characterController.isGrounded)
        {
            jumpDeducted = false;
        }
    }

    #endregion
}