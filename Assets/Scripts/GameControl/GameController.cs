using UnityEngine;

public class GameController : MonoBehaviour
{
    public PerkManager perkManager;   // Reference to the PerkManager
    public bool isGameRunning = false;  // To track if the game is already running

    // Method to start the game (triggered by the start button)
    public void StartGame()
    {
        if (!isGameRunning)
        {
            Debug.Log("Game started!");
            isGameRunning = true;

            // Loop through all the active slots and apply perks in order
            foreach (Slot slot in perkManager.activeSlots)
            {
                // Get the Perk in this slot
                Perk perkInSlot = slot.GetComponentInChildren<Perk>();

                // If there is a perk in this slot and it's active, apply its effect
                if (perkInSlot != null && perkInSlot.isActive)
                {
                    perkInSlot.ApplyEffect();
                }
                else
                {
                    Debug.Log("No active perk in slot: " + slot.name);
                }
            }

            // After applying the effects, you can trigger further gameplay logic
            OnGameStarted();
        }
        else
        {
            Debug.Log("Game is already running.");
        }
    }

    // Called after the game starts and perks are applied
    private void OnGameStarted()
    {
        // Add any post-start logic here (e.g., countdowns, player movement, etc.)
        Debug.Log("All perks applied, game logic starts.");
    }

    // Method to reset the game (optional)
    public void ResetGame()
    {
        if (isGameRunning)
        {
            Debug.Log("Game reset.");
            isGameRunning = false;

            // Clear any applied perk effects or reset game state here
            foreach (Slot slot in perkManager.activeSlots)
            {
                Perk perkInSlot = slot.GetComponentInChildren<Perk>();
                if (perkInSlot != null)
                {
                    // Optionally, you could deactivate or reset each perk here
                    perkInSlot.isActive = false;  // Deactivate the perk
                    Debug.Log(perkInSlot.perkData.perkName + " deactivated.");
                }
            }

            // Reset additional game logic here if needed
        }
        else
        {
            Debug.Log("Game is not running.");
        }
    }
}
