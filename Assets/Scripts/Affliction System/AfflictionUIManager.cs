using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

/// <summary>
/// A manager class that handles the UI for active afflictions.
/// It instantiates and updates UI elements for each active affliction.
/// </summary>
public class AfflictionUIManager : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// The prefab to use for the UI elements representing afflictions.
    /// It should contains a sprite for the affliction icon, a text field for the name, and a text field for the duration.
    /// </summary>
    [Tooltip("The prefab to use for the UI elements representing afflictions.")]
    public GameObject afflictionUIPrefab;

    /// <summary>
    /// The parent transform under which the affliction UI elements will be instantiated.
    /// </summary>
    [Tooltip("The parent transform under which the affliction UI elements will be instantiated.")]
    public Transform afflictionUIParent;

    /// <summary>
    /// A dictionary of active affliction UI elements, keyed by the affliction name.
    /// </summary>
    private Dictionary<string, GameObject> activeAfflictionUI = new Dictionary<string, GameObject>();

    /// <summary>
    /// A list of the current active afflictions <see cref="IAffliction"/>.
    /// Based on the current afflictions in the <see cref="AfflictionManager"/>.
    /// </summary>
    private List<IAffliction> currentAfflictions;

    #endregion

    private void Start()
    {
        if(afflictionUIPrefab == null)
        {
            Debug.LogError("Affliction UI Prefab is not set in the inspector!");
        }

        if(afflictionUIParent == null)
        {
            afflictionUIParent = GameObject.Find("Affliction_Content").transform;
        }
    }

    void Update()
    {
        UpdateUI();
    }

    #region Update Method

    /// <summary>
    /// Updates the UI elements for active afflictions.
    /// </summary>
    private void UpdateUI()
    {
        // Retrieve a copy of the current active afflictions.
        currentAfflictions = AfflictionManager.Instance.GetActiveAfflictions();

        // Create or update UI elements for each active affliction (keyed by affliction name).
        foreach (IAffliction aff in currentAfflictions)
        {
            if (!activeAfflictionUI.ContainsKey(aff.Name))
            {
                // Instantiate a new UI element for the affliction.
                GameObject uiElement = Instantiate(afflictionUIPrefab, afflictionUIParent);

                // Set the name of the UI element to match the affliction.
                uiElement.name = aff.Name;

                // Get the image, name and duration text fields from the UI element.
                Image iconImage = uiElement.transform.Find("Affliction_Icon").GetComponent<Image>();
                TMP_Text nameText = uiElement.transform.Find("Affliction_Name").GetComponent<TMP_Text>();
                TMP_Text durationText = uiElement.transform.Find("Affliction_Duration").GetComponent<TMP_Text>();

                // Set the icon, name and duration text.
                iconImage.sprite = aff.Icon;
                nameText.text = aff.Name;
                durationText.text = aff.Duration.ToString("F1");

                // Add the UI element to the dictionary.
                activeAfflictionUI.Add(aff.Name, uiElement);
            }
        }

        // Update UI elements and remove those for afflictions that are no longer active.
        List<string> keys = new List<string>(activeAfflictionUI.Keys);

        // Check if any active afflictions are no longer active.
        foreach (string key in keys)
        {
            // If the affliction is no longer active, remove the UI element.
            if (!currentAfflictions.Any(a => a.Name == key))
            {
                Destroy(activeAfflictionUI[key]);
                activeAfflictionUI.Remove(key);
            }
            else // Otherwise, update the duration text.
            {
                GameObject uiElement = activeAfflictionUI[key];
                TMP_Text durationText = uiElement.transform.Find("Affliction_Duration").GetComponent<TMP_Text>();
                durationText.text = AfflictionManager.Instance.ReturnAfflictionByName(key).Duration.ToString("F1");
            }
        }
    }

    #endregion
}