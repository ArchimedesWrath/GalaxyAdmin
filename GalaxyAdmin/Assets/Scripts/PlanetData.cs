using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData", menuName = "ScriptableObjects/Planet")]
public class PlanetData : ScriptableObject
{
    public string ID;
    public string Type;
    public float Ice;
    public float Water;
    public float Gas;
    public float Stone;
    public float Metal;
    public float Life;
    public Sprite Sprite;
    public Sprite SecondarySprite;
    public float MaxCycle;
    public int Level;
}
