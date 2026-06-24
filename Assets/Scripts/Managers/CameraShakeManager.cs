using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void CameraShake(GameObject ob, float intensity)
    {

        CinemachineImpulseSource impulseSource = ob.GetComponent<CinemachineImpulseSource>();

        float X = Random.Range(-0.1f, 0.1f);
        float Y = Random.Range(-0.1f, 0.1f);

        impulseSource.DefaultVelocity = new Vector3(X, Y, 0f);

        impulseSource.GenerateImpulseWithForce(intensity * 10f);
    }
}
