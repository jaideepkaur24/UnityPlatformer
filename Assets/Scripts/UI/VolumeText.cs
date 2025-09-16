using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;  // "soundVolume" or "musicVolume"
    [SerializeField] private string textIntro;   // "Sound: " or "Music: "
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        // Multiply by 100 to show as percentage
        float volumeValue = PlayerPrefs.GetFloat(volumeName, 1f) * 100;
        txt.text = textIntro + volumeValue.ToString("F0");
    }
}
