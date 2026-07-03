using System;
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

    public static event Action<float> OnMouseChanged;
    public static event Action<float> OnFOVChanged;
    public static event Action<float> OnAudioChanged;

    private void Start()
    {
        // Load saved values
        float m = PlayerPrefs.GetFloat("Mvalue", 5);
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

        //sent event
        OnMouseChanged?.Invoke(m);
        OnFOVChanged?.Invoke(f);
        OnAudioChanged?.Invoke(a);
    }

    private void HandleMouse(float v)
    {
        PlayerPrefs.SetFloat("Mvalue", v);
        OnMouseChanged?.Invoke(v);
        UpdateText(Mousevalue, v);
    }

    private void HandleFOV(float v)
    {
        PlayerPrefs.SetFloat("Fvalue", v);
        OnFOVChanged?.Invoke(v);
        UpdateText(FOVvalue, v);
    }

    private void HandleAudio(float v)
    {
        PlayerPrefs.SetFloat("Avalue", v);
        OnAudioChanged?.Invoke(v);
        AudioListener.volume = v;
        UpdateText(Audiovalue, v);
    }

    private void UpdateText(TextMeshProUGUI obj, float value)
    {
        obj.text = Mathf.Floor(value).ToString();
    }
}
