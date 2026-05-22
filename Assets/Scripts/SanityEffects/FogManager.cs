using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private float _threshold;
    [SerializeField] private float _defaultFogAmount;
    [SerializeField] private float _maxFogAmount;
    private float _barValue;
    private void Start()
    {
        RenderSettings.fogDensity = _defaultFogAmount;
    }
    public void UpdateFogAmount(float amount)
    {
        float fogAmount = _defaultFogAmount;
        _barValue = amount;

        if (_barValue < _threshold)
        {
            float t = Mathf.InverseLerp(60f, 0f, _barValue);

            fogAmount = Mathf.Lerp(_defaultFogAmount, _maxFogAmount, t);
        }

        RenderSettings.fogDensity = fogAmount;
    }
}
