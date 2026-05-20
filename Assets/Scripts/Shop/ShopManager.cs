using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<UpgradeObject> TotalUpgrades;
    private List<UpgradeObject> _availableUpgrades = new List<UpgradeObject>();
    private List<UpgradeObject> _assignedUpgrades = new List<UpgradeObject>();

    public List<GameObject> Buttons;

    private PlayerHealth player;
    private SanityManager sanityManager;

    public static event Action<string> OnShopMessage;
    public static event Action<bool> OnShopOpenClose;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        sanityManager = FindFirstObjectByType<SanityManager>();

        //fill available upgrades list
        _availableUpgrades = new List<UpgradeObject> (TotalUpgrades);

    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;

        //stop sanity bar from draining
        OnShopOpenClose?.Invoke(false);

        for (int i = 0; i < Buttons.Count; i++)
        {

            TextMeshProUGUI text = Buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            //assign text
            if (_availableUpgrades.Count > 0)
            {
                //find upgrade to display
                int index = UnityEngine.Random.Range(0, _availableUpgrades.Count - 1);

                var upgrade = _availableUpgrades[index];

                text.text = upgrade.Title;

                //add to assigned buttons
                _assignedUpgrades.Add(upgrade);


                //add correct buy function to buttons
                Button button = Buttons[i].GetComponent<Button>();
                button.onClick.AddListener(() => BuyUpgrade(upgrade));


                //remove upgrade from available list (prevent assigning the same button)
                _availableUpgrades.RemoveAt(index);
            }
            else //if no upgrades left
            {
                text.text = "Sold Out";

                for (int j = 0; j < Buttons.Count; j++)
                {
                    Button button = Buttons[i].GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                }
            }
        }
    }


    //refill available if no upgrade bought
    public void SkipShop()
    {
        for (int i = 0; i < _assignedUpgrades.Count; i++)
        {
            _availableUpgrades.Add(_assignedUpgrades[i]);
        }

        //clear list
        _assignedUpgrades.Clear();

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //continue draining bar
        OnShopOpenClose?.Invoke(true);
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


            if(_assignedUpgrades.Count > 1)
            {
                //find other upgrade
                _assignedUpgrades.Remove(upgrade);
                //add so other upgrade is back in pool
                _availableUpgrades.Add(_assignedUpgrades[0]);
            }


            //clear list
            _assignedUpgrades.Clear();

            gameObject.SetActive(false);
        }
        else
        {
            FadeInMessage?.Invoke("Not enough Sanity");
        }
    }
}
