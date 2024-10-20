using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI perkNameText; // Text component for the perk name
    [SerializeField] private TextMeshProUGUI perkCostText;  // Text component for the perk cost
    [SerializeField] private TextMeshProUGUI perkDescriptionText; // Text component for the perk description
    private PerkData perkData; // Store the PerkData

    public void Initialize(PerkData data)
    {
        perkData = data; // Assign the PerkData
        perkNameText.text = perkData.perkName; // Set the name text
        perkCostText.text = $"Cost: {perkData.cost}"; // Set the cost text
        perkDescriptionText.text = perkData.description; // Set the description text
    }

    public void OnClick()
    {
        // Handle what happens when this perk button is clicked (e.g., purchase the perk)
        Debug.Log($"Clicked on perk: {perkData.perkName}");
        // Implement your purchase logic here
    }
}
