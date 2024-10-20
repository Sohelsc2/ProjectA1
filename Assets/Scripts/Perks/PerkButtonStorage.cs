using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkButtonStorage : MonoBehaviour
{


    //public TextMeshProUGUI perkNameText; // Reference to UI element for the perk name
    //public Image perkIcon; // Reference to UI element for the perk icon

    public void Initialize(PerkData perkData)
    {
        if (perkData == null) 
        {
            Debug.LogError("PerkData is null in PerkButton.Initialize!");
            return;
        }

        // Set the UI elements to display the perk's data
        //perkNameText.text = perkData.perkName;
        // perkIcon.sprite = perkData.perkIcon; // Set the icon, assuming you have one
    }

    public void OnClick()
    {
        // Handle what happens when this perk button is clicked (e.g., purchase the perk)
        Debug.Log($"Clicked on perk in storage");
        // Implement your purchase logic here
    }
}
