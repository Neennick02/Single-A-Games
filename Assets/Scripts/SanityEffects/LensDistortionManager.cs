using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LensDistortionManager : MonoBehaviour
{
    public static bool Distort;
    private Volume _volume;
    private VolumeProfile profile;
    private LensDistortion lensDistortion;

    public float _currentValue = 0;
    [SerializeField] private float _distortionRange = 0.5f;
    [SerializeField] private float _speed = 0.5f;
    private float timer = 0f;
    private void Start()
    {
        _volume = GetComponent<Volume>();
        profile = _volume.profile;
        if (!profile.TryGet(out lensDistortion))
        {
            enabled = false;
            Debug.LogWarning("Can't find Lens Distortion Volume Component.");
            return;
        }
    }
    private void Update()
    {
        if (Distort)
        {
            timer += Time.deltaTime;
            _currentValue = Mathf.PingPong(timer * _speed, _distortionRange);
            lensDistortion.intensity.value = _currentValue;
        }
    }
}
