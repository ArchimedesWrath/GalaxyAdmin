using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "ScriptableObjects/Item/Material")]
public class MaterialData : ScriptableObject
{
    public string ID;
    public string type;
    public float cost;
    public float amount;
}
