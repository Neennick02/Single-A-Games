using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Explainers : MonoBehaviour
{
    [SerializeField] public List<Image> Images;

    private void OnEnable()
    {
        int randomIndex = Random.Range(0, Images.Count);
        Images[randomIndex].enabled = true;
        Debug.Log("on");
    }

    private void OnDisable()
    {
        foreach (var image in Images)
        {
            if (image.enabled)
            {
                image.enabled = false;
            }
        }
        Debug.Log("off");


    }

}

