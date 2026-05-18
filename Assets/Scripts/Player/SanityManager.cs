using System;
using System.Collections;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static float MaxSanity;
    private float _sanityAmount = 100;

    [SerializeField] float _drainRate = 1f;
    private bool _draining = true;
    public static event Action<float> OnDrainAmountChanged;

    [SerializeField] private float _damageAmount;
    [SerializeField] private float _damageInterval;
    [SerializeField] private float _damageThreshhold;
    public static event Action<float> OnTakeSanityDamage;
    private float timer = 0;

    //effects events
    private FogManager _fogManager;

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
            _sanityAmount -= _drainRate * Time.deltaTime;
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
        float fogAmount = (MaxSanity - _sanityAmount) / 270;
        _fogManager.UpdateFogAmount(fogAmount);
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
