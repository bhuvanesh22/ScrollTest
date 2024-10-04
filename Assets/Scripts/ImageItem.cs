using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageItem : MonoBehaviour
{
    public Image imageComponent;     // The UI Image component
    public TextMeshProUGUI indexText;           // The UI Text component to display the index
    public int index;                // Index of the image

    // Set the index text
    public void SetText(string text)
    {
        indexText.text = text;
    }

    // Set the image sprite
    public void SetImage(Sprite sprite)
    {
        imageComponent.sprite = sprite;
    }
}
