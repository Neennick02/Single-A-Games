using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EyeGrenade : MonoBehaviour
{
    public GameObject GrenadePrefab;
    private bool _canUseEye = true;
    public bool _isHolding = false;

    [SerializeField] private Transform _throwPoint;

    public static event Action OnBlindEye;
    public static event Action OnHealEye;

    [SerializeField] private float _eyeRegenerationTime = 5f;
    private float timer;


    public GameObject grenade;
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
        if (!_isHolding && _canUseEye && grenade == null)
        {
            Grab();
        }
        else if (_isHolding && grenade != null)
        {
            Throw();
        }
    }

    private void Grab()
    {
        _canUseEye = false;
        _isHolding = true;

        OnBlindEye?.Invoke();

        //spawn grenade
        grenade = Instantiate(GrenadePrefab, _throwPoint);
        grenade.transform.parent = _throwPoint;

        //assign script
        Grenade script = grenade.GetComponent<Grenade>();
        script.parentScript = this;


        //effect
    }

    private void Throw()
    {
        //empty hands
        grenade.transform.parent = null;
        _isHolding = false;

        Rigidbody rb = grenade.GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        //add force
        rb.AddForce(_throwForce * _throwPoint.forward, ForceMode.Impulse);
    }
}


