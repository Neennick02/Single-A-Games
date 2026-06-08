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

    private bool _isDead;

    private CinemachineImpulseSource _impulseSource;

    [SerializeField] private CameraShakeManager _cameraShakeManager;

    [SerializeField] private Volume _pukeEffect;

    private void Start()
    {
        _barImage = GetComponent<Image>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();
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
        }

        else if (_isPuking && _intensity > 0)
        {

            _pukeEffect.weight = _intensity;

            _barImage.fillAmount = _intensity -= Time.deltaTime * 0.3f;


            _cameraShakeManager.CameraShake(_impulseSource, _intensity);

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
}
