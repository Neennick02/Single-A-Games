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

    [SerializeField] private float _eyeRegenerationTime = 5f;
    private float timer;


    GameObject grenade;
    [SerializeField] private float _throwForce = 1;

    private void OnEnable()
    {
        InputManager.OnThrowGrenade += Throw;

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
                _canUseEye = true;
            }
        }
    }

    public void Throw()
    {
        if (!_isHolding)
        {
            Grab();
        }
        else
        {
            StartCoroutine(ThrowRoutine());
        }
    }

    private void Grab()
    {
        _canUseEye = false;
        grenade = Instantiate(GrenadePrefab, _throwPoint);
        grenade.transform.parent = _throwPoint;
        _isHolding = true;
    }

    IEnumerator ThrowRoutine()
    {
        Rigidbody rb = grenade.GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(_throwForce * _throwPoint.forward, ForceMode.Impulse);
        Debug.Log("throw");
        _isHolding = false;
        yield return null;
    }
}


