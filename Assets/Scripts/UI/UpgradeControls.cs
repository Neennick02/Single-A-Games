using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeControls : MonoBehaviour
{
    public GameObject PukeIcon;
    public GameObject EyeIcon;
    public float YOffSet = 150;


    private void OnEnable()
    {
        DisplayUpgrades();
        PukeUpgrade.OnGrabVomit += DisplayUpgrades;
        GameManager.OnEnableEyeUI += DisplayUpgrades;
    }

    private void OnDisable()
    {
        PukeUpgrade.OnGrabVomit -= DisplayUpgrades;
        GameManager.OnEnableEyeUI -= DisplayUpgrades;
    }

    private void DisplayUpgrades()
    {
        int index = 0;

        PukeIcon.SetActive(false);
        EyeIcon.SetActive(false);

        if (GameManager.Instance.HasVomit)
        {
            PukeIcon.SetActive(true);
            PukeIcon.transform.localPosition = new Vector3(200, -index * YOffSet, 0);
            index++;
        }

        if (GameManager.Instance.HasEye)
        {
            EyeIcon.SetActive(true);
            EyeIcon.transform.localPosition = new Vector3(200, -index * YOffSet, 0);
            index++;
        }
    }
}
