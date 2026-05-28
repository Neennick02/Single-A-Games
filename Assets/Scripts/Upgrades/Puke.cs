using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private void Start()
    {
        _barImage = GetComponent<Image>();
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
