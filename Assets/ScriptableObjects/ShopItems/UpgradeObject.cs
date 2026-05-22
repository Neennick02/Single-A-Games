using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeObject", menuName = "Scriptable Objects/UpgradeObject")]
public class UpgradeObject : ScriptableObject
{
    public byte Price;
    public GameObject Prefab;

    public string Title;
    public string Description;
}
