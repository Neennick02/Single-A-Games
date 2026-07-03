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

    public List<GameObject> ItemSlots;

    private PlayerHealth player;
    private SanityManager sanityManager;

    public static event Action<string> OnShopMessage;
    public static event Action<bool> OnShopOpenClose;


    public AudioClip BuyAudio;
    public AudioClip ButtonAudio;

    private PlayerAnimator playerAnimator;
    public UpgradeControls Controls;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        sanityManager = FindFirstObjectByType<SanityManager>();
        playerAnimator = FindFirstObjectByType<PlayerAnimator>();

    }
    private void OnEnable()
    {
        _availableUpgrades = new List<UpgradeObject>(TotalUpgrades);
        _assignedUpgrades.Clear();

        _availableUpgrades.RemoveAll(u => GameManager.Instance.HasUpgrade(u));

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

        for (int j = 0; j < ItemSlots.Count; j++)
        {
            Button button = ItemSlots[j].GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
        }

        //stop sanity bar from draining
        OnShopOpenClose?.Invoke(false);

        for (int i = 0; i < ItemSlots.Count; i++)
        {
            TextMeshProUGUI title = ItemSlots[i].GetComponentInChildren<TextMeshProUGUI>();
            Button button = ItemSlots[i].GetComponentInChildren<Button>();
            ShopItem item = ItemSlots[i].GetComponent<ShopItem>();

            if (_availableUpgrades.Count == 0)
            {
                title.text = "Sold Out";
                button.onClick.RemoveAllListeners();
                item.AssignPrice(null);
                item.AssignSprite(null);
                item.AssignTitle("Sold Out");  
                continue;
            }

            int index = UnityEngine.Random.Range(0, _availableUpgrades.Count);
            var upgrade = _availableUpgrades[index];

            title.text = upgrade.Title;
            _assignedUpgrades.Add(upgrade);

            button.onClick.AddListener(() => BuyUpgrade(upgrade));

            item.AssignPrice(upgrade.Price.ToString());
            item.AssignSprite(upgrade.Image);
            item.AssignTitle(upgrade.Title);

            _availableUpgrades.RemoveAt(index);
        }
    }


    //refill available if no upgrade bought
    public void SkipShop()
    {
        _assignedUpgrades.Clear();

        for (int j = 0; j < ItemSlots.Count; j++)
        {
            Button button = ItemSlots[j].GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
        }

        Time.timeScale = 1;
        gameObject.SetActive(false);
        AudioManager.Instance.PlayClip(ButtonAudio);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //continue draining bar
        OnShopOpenClose?.Invoke(true);
    }

    public void BuyUpgrade(UpgradeObject upgrade)
    {
        if (sanityManager.SanityAmount >= upgrade.Price)
        {
            sanityManager.AddSanity(-upgrade.Price);

            if (playerAnimator._state != 3)
                playerAnimator._state++;

            GameObject upgradeObject = Instantiate(upgrade.Prefab);
            upgradeObject.transform.parent = player.transform;
            GameManager.Instance.AddUpgrade(upgrade);
            OnShopMessage?.Invoke(upgrade.Description);

            _assignedUpgrades.Clear();

            Time.timeScale = 1;
            gameObject.SetActive(false);

            AudioManager.Instance.PlayClip(ButtonAudio);
            AudioManager.Instance.PlayClip(BuyAudio);
            Controls.DisplayUpgrades();
        }
        else
        {
            OnShopMessage?.Invoke("Not enough Sanity");
            AudioManager.Instance.PlayClip(ButtonAudio);
        }
    }
}
