using Unity.Cinemachine;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    Transform target;
    private void Start()
    {
        target = PlayerMotor.Instance.gameObject.transform;
    }
    void Update()
    {
        if(target != null)
        transform.LookAt(target.position);
    }
}
