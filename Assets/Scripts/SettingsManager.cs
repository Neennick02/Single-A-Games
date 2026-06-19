using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private float _fov;
    private float _mouseSensitivity;
    private float _masterVolume;

    public Slider FovSlider;
    public Slider MouseSlider;
    public Slider VolumeSlider;

    private void Start()
    {
        FovSlider.onValueChanged.AddListener(delegate { SetFov(); });
       // MouseSlider.onValueChanged.AddListener(delegate { SetMouse(); });
        VolumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });

        AudioListener.volume = PlayerPrefs.GetFloat("Volume", _masterVolume);

    }

    private void SetFov()
    {
        _fov = FovSlider.value;
        PlayerPrefs.SetFloat("FOV", _fov);
    }

 /*   private void SetMouse()
    {
        _mouseSensitivity = MouseSlider.value;
        PlayerPrefs.SetFloat("Mouse", _mouseSensitivity);
    }*/

    private void SetVolume()
    {
        _masterVolume = VolumeSlider.value;
        PlayerPrefs.SetFloat("Volume", _masterVolume);
        AudioListener.volume = _masterVolume;
    }
}
