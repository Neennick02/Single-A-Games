using System;
using UnityEngine;
using UnityEngine.UI;

public class PukeUpgrade : MonoBehaviour
{
    public static event Action OnGrabVomit;
    void Start()
    {
        OnGrabVomit?.Invoke();
    }
}
