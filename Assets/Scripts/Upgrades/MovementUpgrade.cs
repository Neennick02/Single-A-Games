using UnityEngine;

public class MovementUpgrade : MonoBehaviour
{
    public float SpeedIncrease = 1.5f;
    public float DrainRateIncrease = 1.5f;
    private PlayerMovement movement;
    private SanityManager manager;
    private void Start()
    {
        movement = GetComponentInParent<PlayerMovement>();
        manager = FindFirstObjectByType<SanityManager>();

        movement.IncreaseMultiplier(SpeedIncrease);
        manager.IncreaseDrainAmount(DrainRateIncrease);
    }

    private void OnDestroy()
    {
        movement.IncreaseMultiplier(1);
        manager.IncreaseDrainAmount(1);
    }
}
