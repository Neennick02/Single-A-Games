using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float HealAmount = 1;

    private void Update()
    {
        float y = Mathf.PingPong(Time.time / 5, 0.2f);
        transform.position = new Vector3(transform.position.x, 2 + y, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();

            //add health
            if (health != null) health.Heal(HealAmount);

            //effect

            //sound

            Destroy(transform.parent.gameObject);
        }
    }
}
