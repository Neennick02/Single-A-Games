using NUnit.Framework.Internal;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private float lookSensitivity;
    float minFov ;
    float maxFov ;
    
    CinemachineCamera _cam;
    
    float xRotation = 0f;
    float yRotation = 0f;

    public float SensitivityMultiplier;
    private void Awake()
    {
        _cam = GetComponentInChildren<CinemachineCamera>();
        SettingsMenu.OnFOVChanged += ChangeFov;
        SettingsMenu.OnMouseChanged += ChangeMouse;
        lookSensitivity = PlayerPrefs.GetFloat("Mvalue", 0.5f) ;
    }
    private void OnDisable()
    {
        SettingsMenu.OnFOVChanged -= ChangeFov;
        SettingsMenu.OnMouseChanged -= ChangeMouse;
    }

    private void ChangeFov(float v)
    {
        minFov = v;
        maxFov = v * 1.45f;
        _cam.Lens.FieldOfView = v; 
    }
    private void ChangeMouse(float v)
    {
        lookSensitivity = v * 0.1f ;
        
    }
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * lookSensitivity;
        float mouseY = input.y * lookSensitivity ;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            mouseX *= 0.1f;
            mouseY *= 0.1f;
        }


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
