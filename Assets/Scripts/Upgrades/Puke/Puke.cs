using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Puke : MonoBehaviour
{

    private Image _barImage;

    private float _intensity;

    private PlayerInput _inputActions;

    private bool _isPuking;

    private bool _pukeBegun;

    private bool _isDead;

    private float _pukeDuration = 0.1f;

    private CinemachineImpulseSource _impulseSource;

    [SerializeField] private GameObject _camera;

    [SerializeField] private GameObject _pukeProjectile;

    [SerializeField] private CameraShakeManager _cameraShakeManager;

    [SerializeField] private Volume _pukeEffect;

    private void Start()
    {
        _barImage = GetComponent<Image>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();

        _camera = Camera.main.gameObject;

    }

    private void OnEnable()
    {

        if (_inputActions == null)
        {
            _inputActions = new PlayerInput();
        }

        _inputActions.Enable();

        _inputActions.OnFoot.Puke.performed += Puking;
        _inputActions.OnFoot.Puke.canceled += Puking;

        PlayerHealth.OnDeath += Die;
    }

    private void OnDisable()
    {
        _inputActions.OnFoot.Puke.performed -= Puking;
        _inputActions.OnFoot.Puke.canceled -= Puking;

        PlayerHealth.OnDeath -= Die;
        _inputActions.Disable();
    }

    void Update()
    {

        _pukeDuration = (1 - _intensity) * 0.1f;

        if (!_isPuking)
        {

            _pukeEffect.weight = Mathf.Lerp(_pukeEffect.weight, 0, Time.deltaTime * 5f);

            if (_intensity < 1f)
            {
                _barImage.fillAmount = _intensity += Time.deltaTime * 0.1f;
            }

            else
            {
                _intensity = 1f;
            }

            _pukeBegun = false;

        }

        else if (_isPuking && _intensity > 0)
        {

            _pukeEffect.weight = _intensity;

            _barImage.fillAmount = _intensity -= Time.deltaTime * 0.3f;

            if (!_pukeBegun)
            {
                _pukeBegun = true;
                StartCoroutine(PukeShake());
                StartCoroutine(PukeShoot());
            }
        }


    }

    private void Puking(InputAction.CallbackContext context)
    {

        _isPuking = context.performed;

    }


    private void Die()
    {
        _isDead = true;
    }

    private IEnumerator PukeShake()
    {

        while (_isPuking && _intensity > 0)
        {

            float X = Random.Range(-0.1f, 0.1f);
            float Y = Random.Range(-0.1f, 0.1f);

            _impulseSource.DefaultVelocity = new Vector3(X, Y, 0f);

            _cameraShakeManager.CameraShake(_impulseSource, _intensity);

            yield return new WaitForSeconds(0.1f);

        }

    }

    private IEnumerator PukeShoot()
    {
        while (_isPuking && _intensity > 0)
        {
            float RandomXOffset = Random.Range(-0.5f, 0.5f);
            float RandomYOffset = Random.Range(-0.5f, 0.5f);

            Instantiate(_pukeProjectile, _camera.transform.position + new Vector3(RandomXOffset, RandomYOffset, 0f), _camera.transform.rotation = Quaternion.Euler(_camera.transform.rotation.eulerAngles + new Vector3(RandomXOffset * 30, RandomYOffset * 30, 0f)));

            yield return new WaitForSeconds(_pukeDuration);
        }
    }
}
