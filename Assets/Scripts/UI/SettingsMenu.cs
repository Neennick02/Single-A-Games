using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TextMeshProUGUI Mousevalue;
    public TextMeshProUGUI FOVvalue;
    public TextMeshProUGUI Audiovalue;

    public Slider MouseSlider;
    public Slider FOVSlider; 
    public Slider AudioSlider;

    private float Mvalue;
    private float Fvalue;
    private float Avalue;

    private void Start()
    {
        // Load saved values
        float m = PlayerPrefs.GetFloat("Mvalue", 30);
        float f = PlayerPrefs.GetFloat("Fvalue", 60);
        float a = PlayerPrefs.GetFloat("Avalue", 50);

        // Apply to sliders
        MouseSlider.value = m;
        FOVSlider.value = f;
        AudioSlider.value = a;

        // Update text
        UpdateText(Mousevalue, m);
        UpdateText(FOVvalue, f);
        UpdateText(Audiovalue, a);

        // Add listeners
        MouseSlider.onValueChanged.AddListener(HandleMouse);
        FOVSlider.onValueChanged.AddListener(HandleFOV);
        AudioSlider.onValueChanged.AddListener(HandleAudio);
    }

    private void HandleMouse(float v)
    {
        PlayerPrefs.SetFloat("Mvalue", v);
        UpdateText(Mousevalue, v);
    }

    private void HandleFOV(float v)
    {
        PlayerPrefs.SetFloat("Fvalue", v);
        UpdateText(FOVvalue, v);
    }

    private void HandleAudio(float v)
    {
        PlayerPrefs.SetFloat("Avalue", v);
        UpdateText(Audiovalue, v);
    }

    private void UpdateText(TextMeshProUGUI obj, float value)
    {
        obj.text = Mathf.Floor(value).ToString();
    }
}
