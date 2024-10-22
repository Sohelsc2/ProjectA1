using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    private Image buttonImage;       // Reference to the button's Image component
    private Image highlightImage;    // The dynamically created highlight Image
    private float highlightDuration; // Duration for the highlight animation

    // Default highlight parameters
    private Color defaultHighlightColor = Color.yellow; // Default color for the highlight
    private float defaultHighlightDuration = 1.0f;      // Default duration for the highlight animation

    void Awake()
    {
        // Get the button's Image component (assuming the button has an Image component)
        buttonImage = GetComponent<Image>();
    }

    // Function to start the highlight animation with default parameters
    public void StartHighlight()
    {
        StartHighlight(defaultHighlightColor, defaultHighlightDuration);
    }

    // Function to start the highlight animation with customizable parameters
    public void StartHighlight(Color highlightColor, float duration)
    {
        highlightDuration = duration; // Set the highlight duration

        // If a highlight image already exists, destroy it to avoid duplication
        if (highlightImage != null)
        {
            Destroy(highlightImage.gameObject);
        }

        // Create the highlight Image as a child of the button
        GameObject highlightObject = new GameObject("HighlightImage");
        highlightObject.transform.SetParent(transform, false); // Attach to the button without changing local transform
        highlightObject.transform.localScale = Vector3.one;    // Ensure the same scale
        highlightObject.transform.localPosition = Vector3.zero; // Align to the button's position

        // Add Image component for the highlight
        highlightImage = highlightObject.AddComponent<Image>();

        // Set the highlight Image's sprite to match the button's sprite
        highlightImage.sprite = buttonImage.sprite;

        // Set the highlight color
        highlightImage.color = highlightColor;

        // Ensure the highlight image exactly matches the button's size and alignment
        RectTransform highlightRect = highlightObject.GetComponent<RectTransform>();
        RectTransform buttonRect = buttonImage.GetComponent<RectTransform>();

        // Copy all the RectTransform settings from the button to the highlight
        highlightRect.anchorMin = buttonRect.anchorMin;
        highlightRect.anchorMax = buttonRect.anchorMax;
        highlightRect.pivot = buttonRect.pivot;
        highlightRect.anchoredPosition = buttonRect.anchoredPosition; // Ensures it's perfectly aligned
        highlightRect.sizeDelta = buttonRect.sizeDelta; // Ensure it matches the button's size

        // Set the glow material using the custom glow shader
        highlightImage.material = new Material(Shader.Find("UI/Glow"));

        // Set up fill method for left-to-right animation
        highlightImage.type = Image.Type.Filled;             // Use filled type
        highlightImage.fillMethod = Image.FillMethod.Horizontal; // Fill from left to right
        highlightImage.fillOrigin = (int)Image.OriginHorizontal.Left; // Start from the left
        highlightImage.fillAmount = 0;                       // Start with no fill

        // Start the highlighting animation
        StartCoroutine(AnimateHighlight());
    }

    // Coroutine to animate the highlight effect over time
    private System.Collections.IEnumerator AnimateHighlight()
    {
        float timer = 0f;

        while (timer < highlightDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / highlightDuration;

            // Animate the fill amount from 0 to 1 over time for smooth left-to-right effect
            highlightImage.fillAmount = Mathf.Lerp(0, 1, progress);

            yield return null;
        }

        // Ensure the highlight reaches full fill at the end
        highlightImage.fillAmount = 1;
    }
}
