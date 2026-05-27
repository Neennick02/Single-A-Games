using System;
using Unity.VisualScripting;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static event Action OnMainSceneLoad;
    private void Start()
    {
        OnMainSceneLoad?.Invoke();
    }
}
