using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    internal static Action<Scene, LoadSceneMode> sceneLoaded;

    private void Awake()
    {

    }
}
