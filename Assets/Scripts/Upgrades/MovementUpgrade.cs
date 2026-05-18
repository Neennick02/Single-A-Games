using UnityEngine;

public class MovementUpgrade : MonoBehaviour
{
    public float DamageIncrease = 1.5f;
    public float DrainRateIncrease = 1.5f;

    private void Start()
    {
        PlayerMotor motor = GetComponent<PlayerMotor>();
        SanityManager manager = FindFirstObjectByType<SanityManager>();

        motor.IncreaseMultiplier(DamageIncrease);
        manager.IncreaseDrainAmount(DrainRateIncrease);
    }
}
