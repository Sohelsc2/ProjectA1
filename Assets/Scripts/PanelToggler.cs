using UnityEngine;

public class PanelToggler : MonoBehaviour
{
    // This method toggles the active state of the GameObject the script is attached to
    public void TogglePanel()
    {
        // Toggle the active state of the current GameObject
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
