using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UpgradeObject", menuName = "Scriptable Objects/UpgradeObject")]
public class UpgradeObject : ScriptableObject
{
    public Sprite Image;
    public byte Price;
    public GameObject Prefab;

    public string Title;
    public string Description;
}
