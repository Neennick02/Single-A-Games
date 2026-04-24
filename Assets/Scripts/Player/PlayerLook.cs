using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float lookSensitivity = 30;
    Camera _cam;
    float xRotation = 0f;
    float yRotation = 0f;
    float xSensitivity = 30f;
    float ySensitivity = 30f;

    private void Start()
    {
        _cam = Camera.main;
    }
    public void ProcessLook(Vector2 input)
    {
        xSensitivity = lookSensitivity;
        ySensitivity = lookSensitivity;

        float mouseX = input.x * xSensitivity * Time.deltaTime;
        float mouseY = input.y * ySensitivity * Time.deltaTime;

        // Apply vertical rotation (pitch) with recoil
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply horizontal rotation (yaw) with recoil
        yRotation += mouseX;

        // Apply rotations
        _cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
