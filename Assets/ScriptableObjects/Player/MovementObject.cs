using UnityEngine;

[CreateAssetMenu(fileName = "MovementObject", menuName = "Scriptable Objects/MovementObject")]
public class MovementObject : ScriptableObject
{
    //move
    public float Speed = 3;
    public float SprintSpeed = 5;

    //slide
    public float SlideSpeed = 10;
    public float SlideDuration;

    public float CrouchSpeed = 1;
    public float CrouchHeight = 0.5f;
    public float DefaultHeight = 1;
    //jump
    public float Gravity = 10f;
    public float JumpHeight = 2;


}
