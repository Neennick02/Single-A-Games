using System;
using System.Collections;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static float MaxSanity;
    private float _sanityAmount = 100;
    [SerializeField] float _drainRate = 1f;
    public static event Action<float> OnDrainAmountChanged;
    private bool _draining = true;

    //effects events


    private void Awake()
    {
        MaxSanity = _sanityAmount;
    }
    private void Update()
    {
        if (_draining)
        {
            _sanityAmount -= _drainRate * Time.deltaTime;
        }
        OnDrainAmountChanged?.Invoke(_sanityAmount);
    }

    public void StopStartDrain(bool active)
    {
        _draining = active;
    }
    public void AddSanity(float amount)
    {
        StartCoroutine(AddRoutine(amount));
    }

    IEnumerator AddRoutine(float amount)
    {
        _draining = false;
        float targetAmount = _sanityAmount + amount;

       while (_sanityAmount <= targetAmount )
        {
            _sanityAmount += 0.1f;

            if(_sanityAmount >= MaxSanity)
            {
                break;
            }

            yield return null;
        }

        _draining = true;
    }
    
}
