using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> Upgrades;
    private PlayerHealth player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
    }

    public void BuyUpgrade(int current)
    {
        GameObject upgrade = Instantiate(Upgrades[current]);
        upgrade.gameObject.transform.parent = player.transform;
    }
}
