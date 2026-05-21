using UnityEngine;

[CreateAssetMenu(fileName = "MovementObject", menuName = "Scriptable Objects/MovementObject")]
public class PlayerObject : ScriptableObject
{
    [Header("Speed")]
    public float Speed = 3;
    public float SprintSpeed = 5;

    [Header("Slide")]
    public float SlideDuration = 1f;
    public float SlideInitialBoost = 10f;
    public float SlideAcceleration = 25f;
    public float DownhillMultiplier = 2f;
    public float MaxSlideSpeed = 20f;

    [Header("Dash")]
    public float DashInitialBoost = 10f;
    public float DashAcceleration = 25f;
    public float MaxDashSpeed = 20f;

    [Header("Height")]
    public float SlideHeight = 0.5f;
    public float DefaultHeight = 1;

    [Header("Jump")]
    public float Gravity = 10f;
    public float JumpHeight = 2;

    [Header("Health")]
    public float MaxHealth;

    [Header("Damage")]
    public float Damage;
}
