using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Item/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string ID;
    public string type;
    public float cost;
    public float amount;
}
