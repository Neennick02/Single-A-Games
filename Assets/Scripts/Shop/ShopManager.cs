using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<UpgradeObject> TotalUpgrades;
    public List<UpgradeObject> AvailableUpgrades;

    public List<GameObject> Buttons;
    public List<UpgradeObject> _assignedUpgrades;

    private PlayerHealth player;
    private SanityManager sanityManager;

    public static event Action<string> OnShopMessage;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        sanityManager = FindFirstObjectByType<SanityManager>();

        //fill available upgrades list
        AvailableUpgrades = new List<UpgradeObject> (TotalUpgrades);

    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;

        for (int i = 0; i < Buttons.Count; i++)
        {

            TextMeshProUGUI text = Buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            //assign text
            if (AvailableUpgrades.Count > 0)
            {
                //find upgrade to display
                int index = UnityEngine.Random.Range(0, AvailableUpgrades.Count - 1);

                var upgrade = AvailableUpgrades[index];

                text.text = AvailableUpgrades[index].Title;

                //add to assigned buttons
                _assignedUpgrades.Add(AvailableUpgrades[index]);


                //add correct buy function to buttons
                Button button = Buttons[i].GetComponent<Button>();
                button.onClick.AddListener(() => BuyUpgrade(AvailableUpgrades[index]));


                //remove upgrade from available list (prevent assigning the same button)
                AvailableUpgrades.RemoveAt(index);
            }
            else //if no upgrades left
            {
                text.text = "Sold Out";
            }
        }
    }


    //refill available if no upgrade bought

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void BuyUpgrade(UpgradeObject upgrade)
    {
        //check if enough sanity
        if(sanityManager.SanityAmount >= upgrade.Price)
        {
            //remove resouces
            sanityManager.AddSanity(-upgrade.Price);

            GameObject upgradeObject = Instantiate(upgrade.Prefab);
            upgradeObject.gameObject.transform.parent = player.transform;
            OnShopMessage?.Invoke(upgrade.Description);


            //find other upgrade
            _assignedUpgrades.Remove(upgrade);
            //add so other upgrade is back in pool
            AvailableUpgrades.Add(_assignedUpgrades[0]);

            //clear list
            _assignedUpgrades.Clear();

            gameObject.SetActive(false);
        }
        else
        {
            OnShopMessage?.Invoke("Not enough Sanity");
        }
    }
}
