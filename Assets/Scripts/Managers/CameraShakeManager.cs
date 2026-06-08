using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{

    public void CameraShake(CinemachineImpulseSource impulseSource, float intensity)
    {
        impulseSource.GenerateImpulseWithForce(intensity * 10f);
    }
}
