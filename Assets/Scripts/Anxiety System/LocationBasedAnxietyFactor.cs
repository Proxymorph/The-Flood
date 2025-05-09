using UnityEngine;

/// <summary>
/// An anxiety factor that increases anxiety based on the distance to a location.
/// Attach this script to a GameObject to make it an anxiety factor - would recommend attaching it to the object that is the source.
/// </summary>
[RequireComponent(typeof(LocationAnxietyGizmo))]
[ExecuteInEditMode]
public class LocationBasedAnxietyFactor : MonoBehaviour, IAnxietyUpFactor
{
    #region Properties

    /// <summary>
    /// The character controller of the player.
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// The transform of the anxiety-inducing location.
    /// </summary>
    [Tooltip("The transform of the anxiety-inducing location.")]
    public Transform anxietySource;

    /// <summary>
    /// The maximum distance over which anxiety builds up.
    /// </summary>
    [Tooltip("The maximum distance over which anxiety builds up.")]
    public float maxDistance = 10f;

    /// <summary>
    /// The base rate at which anxiety builds up when at the source.
    /// </summary>
    [Tooltip("The base rate at which anxiety builds up when at the source.")]
    public float baseIncrease = .2f;

    /// <summary>
    /// The distance between the player and the anxiety source. Used to check against the threshold.
    /// </summary>
    private float playerDistance;

    #endregion

    private void Start()
    {
        if (anxietySource == null)
        {
            anxietySource = this.transform;
        }
        else
        {
            Debug.LogWarning("Anxiety source not set for LocationBasedAnxietyFactor on " + gameObject.name);
        }

        if (characterController == null)
        {
            characterController = FindFirstObjectByType<CharacterController>();
        }
        else
        {
            Debug.LogWarning("Character controller not set for LocationBasedAnxietyFactor on " + gameObject.name);
        }
    }

    void Update()
    {
        // Only run this in play mode. A safety check, because we don't want to run this in edit mode.
        if (Application.isPlaying)
        {
            if (characterController == null)
            {
                characterController = FindFirstObjectByType<CharacterController>();
            }
            else
            {
                Debug.LogWarning("Character controller not set for LocationBasedAnxietyFactor on " + gameObject.name);
            }

            // Calculate the distance between the player and the anxiety source.
            if (characterController != null )
            {
                float playerDistance = Vector3.Distance(characterController.transform.position, anxietySource.position);

                // Register or unregister the anxiety factor based on the distance.
                if (playerDistance > maxDistance)
                {
                    AnxietyManager.Instance.UnregisterUpFactor(this);
                }
                else
                {
                    AnxietyManager.Instance.RegisterUpFactor(this);
                }
            }
        }
    }

    #region IAnxietyUpFactor Implementation

    /// <summary>
    /// Returns the anxiety increase based on the distance to the anxiety source.
    /// </summary>
    /// <returns> Anxiety increase based on distance</returns>
    public float AnxietyIncrease()
    {
        if (anxietySource == null)
        {
            return 0f;
        }

        if (playerDistance > maxDistance)
        {
            return 0f;
        }

        // Anxiety increases the closer you are in a linear scale.
        return baseIncrease * (1f - playerDistance / maxDistance);
    }

    #endregion
}
