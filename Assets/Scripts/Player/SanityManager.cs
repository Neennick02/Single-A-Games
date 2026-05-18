using System;
using System.Collections;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static float MaxSanity;
    private float _sanityAmount = 100;

    [SerializeField] float _drainRate = 1f;
    private float _drainRateMultiplier = 1;
    private bool _draining = true;
    public static event Action<float> OnDrainAmountChanged;

    [Header("Damage Settings")]
    [SerializeField] private float _damageAmount;
    [SerializeField] private float _damageInterval;
    [SerializeField] private float _damageThreshhold;
    public static event Action<float> OnTakeSanityDamage;
    private float timer = 0;

    //fog
    [Header("Effect Settings")]
    [SerializeField] private float _lowSanityRestoreMultiplier = 1.5f;
    [SerializeField] private float _sanityMultiplierThreshold = 60f;
    private FogManager _fogManager;


    //damage increase
    [SerializeField] private float _increaseDamageThreshold = 15;
    [SerializeField] private float _damageMultiplier = 1.5f;
    public static event Action<float> OnDamageOutputChange;

    //lens distortion
    [SerializeField] private float _lensDistortThreshold = 20f;

    private void Awake()
    {
        MaxSanity = _sanityAmount;
        _fogManager = GetComponent<FogManager>();
    }

    #region OnEnable
    private void OnEnable()
    {
        EnemyHealth.OnRestoreSanity += AddSanity;
    }

    private void OnDisable()
    {
        EnemyHealth.OnRestoreSanity -= AddSanity;
    }
    #endregion
    private void Update()
    {
        _sanityAmount = Mathf.Clamp(_sanityAmount, 0, MaxSanity);

        //drain over time
        if (_draining)
        {
            _sanityAmount -= (_drainRate * _drainRateMultiplier) * Time.deltaTime;
        }

        OnDrainAmountChanged?.Invoke(_sanityAmount);

        //check if bar is to low
        if(_sanityAmount <= _damageThreshhold)
        {
            timer += Time.deltaTime;
            if(timer > _damageInterval)
            {
                OnTakeSanityDamage?.Invoke(_damageAmount);
                timer = 0;
            }
        }

        //update fog amount
        float fogAmount = (MaxSanity - _sanityAmount) / 800;
        if(_sanityAmount < _sanityMultiplierThreshold)
        {
            _fogManager.UpdateFogAmount(fogAmount);
        }

        //update damage amount
        if(_sanityAmount <= _increaseDamageThreshold)
        {
            OnDamageOutputChange?.Invoke(_damageMultiplier);
        }
        else
        {
            OnDamageOutputChange?.Invoke(1);
        }

        //update lens distortion
        if(_sanityAmount < _lensDistortThreshold)
        {
            LensDistortionManager.Distort = true;
        }
        else
        {
            LensDistortionManager.Distort = false;
        }
    }

    public void StopStartDrain(bool active)
    {
        _draining = active;
    }
    public void AddSanity(float amount)
    {
        //increase amount if sanity is low
        if(_sanityAmount < MaxSanity / 3)
        {
            amount = amount * _lowSanityRestoreMultiplier;
        }
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
    
    public void IncreaseDrainAmount(float amount)
    {
        _drainRateMultiplier = amount;
    }
}
