using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<MonoBehaviour> Upgrades;
    private PlayerHealth player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
    }

    public void BuyUpgrade(int current)
    {
        MonoBehaviour script = Upgrades[current];
        player.gameObject.AddComponent(script.GetType());
    }
}
