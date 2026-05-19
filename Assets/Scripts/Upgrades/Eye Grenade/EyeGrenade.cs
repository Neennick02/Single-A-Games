using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EyeGrenade : MonoBehaviour
{
    public GameObject GrenadePrefab;
    private bool _canUseEye = true;
    private bool _isHolding = false;

    [SerializeField] private Transform _throwPoint;

    public static event Action OnBlindEye;
    public static event Action OnHealEye;

    [SerializeField] private float _eyeRegenerationTime = 5f;
    private float timer;


    GameObject grenade;
    [SerializeField] private float _throwForce = 1;

    private void OnEnable()
    {
        InputManager.OnThrowGrenade += UseEye;

        //hook firepoint to main cam

        _throwPoint = GameObject.FindGameObjectWithTag("FirePoint").transform;

    }

    private void OnDisable()
    {
        InputManager.OnThrowGrenade -= Throw;
    }
    private void Update()
    {
        if (!_canUseEye)
        {
            timer += Time.deltaTime;
            if(timer > _eyeRegenerationTime)
            {
                OnHealEye?.Invoke();    
                _canUseEye = true;
                timer = 0f;
            }
        }
    }

    public void UseEye()
    {
        if (!_isHolding && _canUseEye)
        {
            Grab();
        }
        else if(_isHolding)
        {
            Throw();
        }
    }

    private void Grab()
    {
        OnBlindEye?.Invoke();
        _canUseEye = false;
        grenade = Instantiate(GrenadePrefab, _throwPoint);
        grenade.transform.parent = _throwPoint;
        _isHolding = true;
    }

    private void Throw()
    {
        //if already exploded in hand
        if (grenade == null)
        {
            _isHolding = false;
            return;
        }

        Rigidbody rb = grenade.GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        grenade.transform.parent = null;
        rb.AddForce(_throwForce * _throwPoint.forward, ForceMode.Impulse);

        _isHolding = false;
    }
}


