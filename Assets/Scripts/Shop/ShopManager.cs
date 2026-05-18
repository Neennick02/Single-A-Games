using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<BaseUpgrade> Upgrades;
    private PlayerHealth player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
    }

    public void BuyUpgrade(int current)
    {
        BaseUpgrade script = Upgrades[current];
        player.gameObject.AddComponent(script.GetType());
    }
}
