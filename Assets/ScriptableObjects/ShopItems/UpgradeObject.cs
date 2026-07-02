using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType { Vomit, Eye, Arm, Bud, Leg }


[CreateAssetMenu(fileName = "UpgradeObject", menuName = "Scriptable Objects/UpgradeObject")]
public class UpgradeObject : ScriptableObject
{
    public Sprite Image;
    public byte Price;
    public GameObject Prefab;

    public string Title;
    public UpgradeType Type;

    public string Description;
}
