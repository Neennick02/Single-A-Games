using UnityEngine;

public class MovementUpgrade : BaseUpgrade
{
    public float DamageIncrease = 1.5f;
    public float DrainRateIncrease = 1.5f;
    private PlayerMotor motor;
    private SanityManager manager;
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        manager = FindFirstObjectByType<SanityManager>();

        motor.IncreaseMultiplier(DamageIncrease);
        manager.IncreaseDrainAmount(DrainRateIncrease);
    }

    private void OnDestroy()
    {
        motor.IncreaseMultiplier(1);
        manager.IncreaseDrainAmount(1);
    }
}
