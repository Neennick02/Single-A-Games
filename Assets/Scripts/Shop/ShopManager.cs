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

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Start()
    {
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

            gameObject.SetActive(false);
        }
        else
        {
            OnShopMessage?.Invoke("Not enough Sanity");
        }
    }
}
