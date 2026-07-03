using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeControls : MonoBehaviour
{
    [SerializeField] public List<GameObject> Icons;


    public float YOffSet = 150;



    private void Start()
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

    public void DisplayUpgrades()
    {
        for (int i = 0; i < Icons.Count; i++)
        {
            Icons[i].SetActive(false);
        }

        List<UpgradeObject> pickedUp = GameManager.Instance.PickedUpUpgrades;

        for (int i = 0; i < pickedUp.Count; i++)
        {
            UpgradeObject currentUpgrade = pickedUp[i];
            int iconIndex = (int)currentUpgrade.Type; 

            if (iconIndex < Icons.Count && Icons[iconIndex] != null)
            {
                Icons[iconIndex].SetActive(true);
                Icons[iconIndex].transform.localPosition = new Vector3(200, -i * YOffSet, 0);
            }
        }
    }
}
