using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private float _defaultFogAmount;
    [SerializeField] private float _maxFogAmount;

    private void Start()
    {
        UpdateFogAmount(_defaultFogAmount);
    }
    public void UpdateFogAmount(float amount)
    {
        //never less than default amount
        if (amount < _defaultFogAmount ||
            amount > _maxFogAmount) return;

        RenderSettings.fogDensity = amount;
    }
}
