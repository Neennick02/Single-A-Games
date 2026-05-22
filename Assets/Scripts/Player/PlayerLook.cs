using Unity.Cinemachine;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float lookSensitivity = 30;
    [SerializeField] float minFov = 60f;
    [SerializeField] float maxFov = 180f;
    
    CinemachineCamera _cam;
    
    float xRotation = 0f;
    float yRotation = 0f;
    float xSensitivity = 30f;
    float ySensitivity = 30f;

    private void Start()
    {
        _cam = GetComponentInChildren<CinemachineCamera>();
    }
    public void ProcessLook(Vector2 input)
    {
        xSensitivity = lookSensitivity;
        ySensitivity = lookSensitivity;

        float mouseX = input.x * xSensitivity * Time.deltaTime;
        float mouseY = input.y * ySensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        yRotation += mouseX;

        // Apply rotations
        _cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void UpdateFOV(float velocity)
    {
        float maxSpeed = 15f;

        float t = Mathf.Clamp01(velocity / maxSpeed);

        float targetFov = Mathf.Lerp(minFov, maxFov, t);

        _cam.Lens.FieldOfView = Mathf.Lerp(
               _cam.Lens.FieldOfView,
               targetFov,
               Time.deltaTime * 8f
           );
    }
}
