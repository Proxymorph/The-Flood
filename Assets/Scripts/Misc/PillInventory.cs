using UnityEngine;
using TMPro;

/// <summary>
/// A simple inventory made specifficaly for handling the anxiety pills.
/// </summary>
public class PillInventory : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Create a singleton, for the script to be accessible from anywhere.
    /// </summary>
    public static PillInventory Instance { get; private set; }

    /// <summary>
    /// A reference to the UI text, that gets updated when pills are taken or used.
    /// </summary>
    [SerializeField]
    private TMP_Text pillCountText;

    [Header("Pill Inventory")]

    /// <summary
    /// The current pill count. These pills are accessible to use by the player.
    /// </summary>
    [Tooltip("Pills currently accessible for use by player.")]
    public int pillCount = 0;

    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(pillCountText == null)
        {
            pillCountText = GameObject.Find("Pill_Counter").GetComponent<TMP_Text>();
        }
    }

    #region Utility Methods

    /// <summary>
    /// A method that adds pills to the inventory.
    /// It also updates the UI with the current pill count.
    /// </summary>
    /// <param name="count"> The number to increase the pill count by. </param>
    public void AddPills(int count)
    {
        pillCount += count;
        Debug.Log($"Added {count} pill(s). Total pills: {pillCount}");

        if (pillCountText != null)
        {
            pillCountText.text = pillCount.ToString();
        }
    }

    #endregion
}