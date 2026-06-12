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

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        sanityManager = FindFirstObjectByType<SanityManager>();
        playerAnimator = FindFirstObjectByType<PlayerAnimator>();

        //fill available upgrades list
        _availableUpgrades = new List<UpgradeObject>(TotalUpgrades);
    }
    private void OnEnable()
    {
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

            //assign text
            if (_availableUpgrades.Count > 0)
            {
                //find upgrade to display
                int index = UnityEngine.Random.Range(0, _availableUpgrades.Count);

                var upgrade = _availableUpgrades[index];

                title.text = upgrade.Title;

                //add to assigned buttons
                _assignedUpgrades.Add(upgrade);


                //add correct buy function to buttons
                Button button = ItemSlots[i].GetComponentInChildren<Button>();
                button.onClick.AddListener(() => BuyUpgrade(upgrade));

                ShopItem item = ItemSlots[i].GetComponent<ShopItem>();
                //assign values
                item.AssignPrice(upgrade.Price.ToString());
                item.AssignSprite(upgrade.Image);
                item.AssignTitle(upgrade.Title.ToString());


                //remove upgrade from available list (prevent assigning the same button)
                _availableUpgrades.RemoveAt(index);
            }
            else //if no upgrades left
            {
                title.text = "Sold Out";

                Button button = ItemSlots[i].GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();

                //empty data
                ShopItem item = ItemSlots[i].GetComponent<ShopItem>();
                item.AssignPrice(null);
                item.AssignSprite(null);
                item.AssignTitle(null);

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

        //clear assigned list
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
        //check if enough sanity
        if (sanityManager.SanityAmount >= upgrade.Price)
        {
            //remove resouces
            sanityManager.AddSanity(-upgrade.Price);

            playerAnimator._state++;
            Debug.Log("Player state: " + playerAnimator._state);

            GameObject upgradeObject = Instantiate(upgrade.Prefab);
            upgradeObject.gameObject.transform.parent = player.transform;
            OnShopMessage?.Invoke(upgrade.Description);


            if (_assignedUpgrades.Count > 1)
            {
                //find other upgrade
                _assignedUpgrades.Remove(upgrade);
                //add so other upgrade is back in pool
                _availableUpgrades.Add(_assignedUpgrades[0]);
            }


            //clear list
            _assignedUpgrades.Clear();
            Time.timeScale = 1;
            gameObject.SetActive(false);
            AudioManager.Instance.PlayClip(ButtonAudio);
            AudioManager.Instance.PlayClip(BuyAudio);


        }
        else
        {
            OnShopMessage?.Invoke("Not enough Sanity");
            AudioManager.Instance.PlayClip(ButtonAudio);
        }
    }
}
