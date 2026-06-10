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
    public AudioClip PukeAudio;

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

        _pukeEffect.weight = _intensity;

        if (!_isPuking)
        {

            if (_intensity < 1f)
            {
                _barImage.fillAmount = _intensity += Time.deltaTime * 0.03f;
            }

            else
            {
                StartCoroutine(ForcedPuke());
            }

            _pukeBegun = false;

        }

        else if (_isPuking && _intensity > 0)
        {

            _barImage.fillAmount = _intensity -= Time.deltaTime * 0.3f;

            if (!_pukeBegun)
            {
                _pukeBegun = true;
                StartCoroutine(PukeShake());
                StartCoroutine(PukeShoot());
            }
        }

        else if (_isPuking && _intensity <= 0)
        {
            _isPuking = false;
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
        AudioManager.Instance.PlayClip(PukeAudio);

        while (_isPuking && _intensity > 0)
        {
            float RandomXOffset = Random.Range(-0.5f, 0.5f);
            float RandomYOffset = Random.Range(-0.5f, 0.5f);

            Instantiate(_pukeProjectile, _camera.transform.position + new Vector3(RandomXOffset, RandomYOffset, 0f), _camera.transform.rotation = Quaternion.Euler(_camera.transform.rotation.eulerAngles + new Vector3(RandomXOffset * 30, RandomYOffset * 30, 0f)));

            yield return new WaitForSeconds(_pukeDuration);
        }
    }

    private IEnumerator ForcedPuke()
    {
        while (_intensity > 0)
        {

            _isPuking = true;

            yield return new WaitForSeconds(0.01f);
        }

        _isPuking = false;
    }
}
