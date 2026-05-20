using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> Upgrades;
    public List<string> UpgradeMessages;
    public List<float> UpgradeCost;

    private PlayerHealth player;
    private SanityManager sanityManager;

    public static event Action<string> OnShopMessage;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        player = FindFirstObjectByType<PlayerHealth>();
        sanityManager = FindFirstObjectByType<SanityManager>();
    }

    public void BuyUpgrade(int current)
    {
        //check if enough sanity
        if(sanityManager.SanityAmount >= UpgradeCost[current])
        {
            //remove resouces
            sanityManager.AddSanity(-current);

            GameObject upgrade = Instantiate(Upgrades[current]);
            upgrade.gameObject.transform.parent = player.transform;
            OnShopMessage?.Invoke(UpgradeMessages[current]);

            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }
        else
        {
            OnShopMessage?.Invoke("Not enough Sanity");
        }
    }
}
