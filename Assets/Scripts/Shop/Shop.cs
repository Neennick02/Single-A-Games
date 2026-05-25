using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject ShopObject;

    private void OnEnable()
    {
        OpenShop.OnOpenShop += Open;
    }

    private void OnDisable()
    {
        OpenShop.OnOpenShop -= Open;
    }
    public void Open()
    {
        ShopObject.SetActive(true);
    }
}
