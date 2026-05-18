using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private float _defaultFogAmount;
    [SerializeField] private float _maxFogAmount;
    public float fog;

    private void Start()
    {
        UpdateFogAmount(_defaultFogAmount);
    }
    public void UpdateFogAmount(float amount)
    {
        //never less than default amount
        if (amount < _defaultFogAmount ||
            amount > _maxFogAmount) return;
        
        fog = amount;

        RenderSettings.fogDensity = amount;
    }
}
