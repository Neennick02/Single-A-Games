using UnityEngine;

[CreateAssetMenu(fileName = "MovementObject", menuName = "Scriptable Objects/MovementObject")]
public class PlayerObject : ScriptableObject
{
    [Header("Speed")]
    public float Speed = 3;
    public float SprintSpeed = 5;

    [Header("Slide")]
    public float SlideSpeed = 10;
    public float SlideDuration;
    public float SlideInterval;

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
