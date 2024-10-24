using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class DiceManager : MonoBehaviour
{
public TextMeshProUGUI dieValueText; // Text to display die value
    public TextMeshProUGUI sumText; // Text to display sum of rolls
    public int totalRolls = 0; // Sum of all rolls
    private List<int> preventedValues = new List<int>(); // List to hold prevented values
    public int rollCount=1;
    // Function to roll the die
    private List<int> rolledValues = new List<int>(); // List to hold prevented values


    public IEnumerator AcknowledgeRoll(int addedValue, int acknowledgedValue)
    {
        foreach (int rolledValue in rolledValues)
        {
            if (rolledValue == acknowledgedValue)
            {
                // Bold and red for 0.5 seconds
                dieValueText.text = addedValue.ToString(); // Update die value text
                Canvas.ForceUpdateCanvases();
                yield return StartCoroutine(HighlightFinalValue(addedValue));
                totalRolls += addedValue;
                sumText.text = "Sum of all rolls: " + totalRolls; // Update sum text            
            }
        }
    }
    public IEnumerator Roll(int minValue, int maxValue)
    {
        for (int roll = 0; roll < rollCount; roll++){
        int rollValue;
        // Roll the die 10 times within 1 second
        for (int i = 0; i < 10; i++)
        {
            rollValue = RollingFunction(minValue, maxValue);

        yield return new WaitForSeconds(0.05f/(float)rollCount); // Wait for a short duration between updates
        }
        // Roll until we get a value that isn't prevented
        do
        {
            rollValue = RollingFunction(minValue, maxValue);
        } while (preventedValues.Contains(rollValue)); // Re-roll if the value matches any prevented value

        // Bold and red for 0.5 seconds
        yield return StartCoroutine(HighlightFinalValue(rollValue));
        FinalizeRoll(rollValue);

    }
    }
    private void FinalizeRoll(int rollValue){
        // Update the total rolls
        rolledValues.Add(rollValue);
        totalRolls += rollValue;
        sumText.text = "Sum of all rolls: " + totalRolls; // Update sum text
    }
private int RollingFunction(int minValue, int maxValue){
        int rollValue;
        rollValue = Random.Range(minValue, maxValue + 1); // Roll the die
        dieValueText.text = rollValue.ToString(); // Update die value text
        Canvas.ForceUpdateCanvases();
        return rollValue;
}
public IEnumerator ForceRoll(int minValue, int maxValue)
    {
        for (int roll = 0; roll < rollCount; roll++){
        int rollValue;
        // Roll the die 10 times within 1 second
        for (int i = 0; i < 10; i++)
        {
           rollValue = RollingFunction(minValue, maxValue);

        // Force a layout refresh to ensure text updates are shown immediately

        yield return new WaitForSeconds(0.05f/(float)rollCount); // Wait for a short duration between updates
        }
        rollValue = RollingFunction(minValue, maxValue);

        // Bold and red for 0.5 seconds
        yield return StartCoroutine(HighlightFinalValue(rollValue));
        FinalizeRoll(rollValue);

    }
    }
    // Function to prevent a specific value
    public void Prevent(int preventedValue)
    {
        if (!preventedValues.Contains(preventedValue)) // Only add if not already in the list
        {
            preventedValues.Add(preventedValue); // Add the value to the list
        }
    }

    public void Reset(){
        Debug.LogWarning($"Resetting DiceManager");
        ClearAllPreventedValues();
        ClearAllRolledValues();
        ResetTotalRoll();
        ResetRollCount();
    }
    // Function to clear a specific prevented value
    public void ClearPreventedValue(int valueToClear)
    {
        preventedValues.Remove(valueToClear); // Remove the specified value from the list
    }
    public void ResetTotalRoll()
    {
        totalRolls=0; // Remove the specified value from the list
    }
    public void ResetRollCount()
    {
        rollCount=1; // Remove the specified value from the list
    }    // Function to clear all prevented values if needed
    public void ClearAllPreventedValues()
    {
        preventedValues.Clear(); // Clear the entire list
    }
    public void ClearAllRolledValues()
    {
        rolledValues.Clear(); // Clear the entire list
    }
private IEnumerator HighlightFinalValue(int finalValue)
{
    // Store original properties
    TMPro.FontStyles originalFontStyle = dieValueText.fontStyle;
    Color originalColor = dieValueText.color;
    float originalFontSize = dieValueText.fontSize;

    // Make text bold and red
    dieValueText.fontStyle = TMPro.FontStyles.Bold;
    dieValueText.color = Color.red;
    dieValueText.fontSize *= 1.3f; // Increase size by 10%

    // Display for 0.5 seconds
    yield return new WaitForSeconds(0.5f/(float)rollCount);

    // Revert changes to original properties
    dieValueText.fontStyle = originalFontStyle;
    dieValueText.color = originalColor; // Change back to the original color
    dieValueText.fontSize = originalFontSize; // Reset size back to original
}

}
