using System;
using System.Collections;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static float MaxSanity = 100;
    public float SanityAmount { get; private set;}

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
        SanityAmount = MaxSanity;
        _fogManager = GetComponent<FogManager>();
    }

    #region OnEnable
    private void OnEnable()
    {
        EnemyHealth.OnRestoreSanity += AddSanity;
        ShopManager.OnShopOpenClose += PauseContinueBar;
    }

    private void OnDisable()
    {
        EnemyHealth.OnRestoreSanity -= AddSanity;
        ShopManager.OnShopOpenClose -= PauseContinueBar;
    }
    #endregion
    private void Update()
    {
        //drain over time
        if (_draining)
        {
            SanityAmount -= (_drainRate * _drainRateMultiplier) * Time.deltaTime;
        }

        SanityAmount = Mathf.Clamp(SanityAmount, 0, MaxSanity);

        OnDrainAmountChanged?.Invoke(SanityAmount);

        //check if bar is to low
        if(SanityAmount <= _damageThreshhold)
        {
            timer += Time.deltaTime;
            if(timer > _damageInterval)
            {
                OnTakeSanityDamage?.Invoke(_damageAmount);
                timer = 0;
            }
        }

        //update fog amount
            _fogManager.UpdateFogAmount(SanityAmount);
        

        //update damage amount
        if(SanityAmount <= _increaseDamageThreshold)
        {
            OnDamageOutputChange?.Invoke(_damageMultiplier);
        }
        else
        {
            OnDamageOutputChange?.Invoke(1);
        }

        //update lens distortion
        if(SanityAmount < _lensDistortThreshold)
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
        if(SanityAmount < MaxSanity / 3)
        {
            amount = amount * _lowSanityRestoreMultiplier;
        }
        StartCoroutine(AddRoutine(amount));
    }

    public IEnumerator AddRoutine(float amount)
    {
        _draining = false;
        float targetAmount = SanityAmount + amount;

       while (SanityAmount <= targetAmount )
        {
            SanityAmount += 0.1f;

            if(SanityAmount >= MaxSanity)
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

    public void PauseContinueBar(bool active)
    {
        _draining = active;
    }
}
