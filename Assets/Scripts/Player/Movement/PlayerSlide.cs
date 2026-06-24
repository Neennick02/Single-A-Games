using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSlide : MonoBehaviour
{
    private CinemachinePositionComposer composer;
    [SerializeField] private PlayerObject data;
    [SerializeField] private Transform _headTarget;

    private CharacterController controller;
    private InputManager input;
    private PlayerMovement movement;
    private PlayerStateMachine state;

    private Vector3 slideVelocity;
    private float slideTimer;
    public ParticleSystem SpeedTrails;
    public AudioClip DashClip;
    public AudioClip SlideClip;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        movement = GetComponent<PlayerMovement>();
        state = GetComponent<PlayerStateMachine>();

        composer = GetComponentInChildren<CinemachinePositionComposer>();
    }

    public void Tick()
    {
        if (state.IsSliding)
            SlideUpdate();
        else
            CheckSlideStart();
    }

    private void CheckSlideStart()
    {
        if(state.IsDead) return;

        Vector3 horizontalVel = controller.velocity;
        horizontalVel.y = 0;

        if (!controller.isGrounded)
        {
            //dash 
            if (input.onFoot.Dash.triggered && horizontalVel.magnitude > 0.1f && CorrectDirection())
            {
                state.SetState(PlayerStateMachine.PlayerStates.Dashing);
                AudioManager.Instance.PlayClip(DashClip, .5f, Random.Range(0.8f, 1.2f));
                SpeedTrails.Play();
            }

        }
        else if (input.onFoot.Dash.triggered && horizontalVel.magnitude > 0.2f && CorrectDirection())
        {
            StartSlide();
            SpeedTrails.Play();
            AudioManager.Instance.PlayClip(SlideClip, .5f, Random.Range(0.8f, 1.2f));
        }
    }

    private void StartSlide()
    {
        state.SetState(PlayerStateMachine.PlayerStates.Sliding);

        Vector3 dir = controller.velocity;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.1f)
            dir = transform.forward;

        dir.Normalize();

        slideVelocity = dir * data.SlideInitialBoost;
        slideTimer = 0f;

        StartCoroutine(ChangeHeight(data.SlideHeight));
    }

    private void SlideUpdate()
    {
        slideTimer += Time.deltaTime;

        Vector3 slopeDir = GetSlopeDirection();
        slopeDir.y = 0;

        slideVelocity += slopeDir * data.SlideAcceleration * Time.deltaTime;

        slideVelocity = Vector3.ClampMagnitude(slideVelocity, data.MaxSlideSpeed);

        movement.AddExternalVelocity(slideVelocity);

        if (slideTimer >= data.SlideDuration) { 
            state.SetState(PlayerStateMachine.PlayerStates.Locomotion);
            StartCoroutine(ChangeHeight(data.DefaultHeight));
        }
    }

    private Vector3 GetSlopeDirection()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
            return Vector3.ProjectOnPlane(slideVelocity.normalized, hit.normal).normalized;

        return slideVelocity.normalized;
    }

    private bool CorrectDirection()
    {
        Vector3 horizontalVel = controller.velocity;
        horizontalVel.y = 0;

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;

        horizontalVel.Normalize();
        camForward.Normalize();

        float alignment = Vector3.Dot(horizontalVel, camForward);

        return alignment > 0.5f;
    }

    public IEnumerator ChangeHeight(float offset)
    {
        float duration = 0.25f;
        float timer = 0f;

        float startY = _headTarget.localPosition.y;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float newY = Mathf.Lerp(startY, offset, timer / duration);

            Vector3 localPos = _headTarget.localPosition;
            localPos.y = newY;
            _headTarget.localPosition = localPos;

            yield return null;
        }
    }
}
