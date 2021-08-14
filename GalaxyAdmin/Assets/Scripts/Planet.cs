using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
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

    public float CurrentCycle = 0;

    public Planet(string ID, 
        string Type, 
        float Ice, 
        float Water, 
        float Gas, 
        float Stone, 
        float Metal, 
        float Life, 
        Sprite Sprite,
        Sprite SecondarySprite,
        float MaxCycle)
    {
        this.ID = ID;
        this.Type = Type;
        this.Ice = Ice;
        this.Water = Water;
        this.Gas = Gas;
        this.Stone = Stone;
        this.Metal = Metal;
        this.Life = Life;
        this.Sprite = Sprite;
        this.SecondarySprite = SecondarySprite;
        this.MaxCycle = MaxCycle;
    }

    public void WaitCycle()
    {
        CurrentCycle += 1;
        CheckCycle();
    }

    public void CheckCycle()
    {
        if (CurrentCycle >= MaxCycle)
        {
            Produce();
        }
    }

    public void Produce()
    {
        switch(Type)
        {
            case "ice":
                Ice += 100 + (10 * MaxCycle);
                Water += 10 * MaxCycle;
                break;
            case "water":
                Water += 100 + (10 * MaxCycle);
                Ice += 10 * MaxCycle;
                break;
            case "gas":
                Gas += 100 + (10 * MaxCycle);
                Stone += 10 * MaxCycle;
                break;
            case "stone":
                Stone += 100 + (10 * MaxCycle);
                Metal += 10 * MaxCycle;
                break;
            case "metal":
                Metal += 50 + (10 * MaxCycle);
                Ice += 10 * MaxCycle;
                break;
            case "Life":
                Life += 20 + (10 * MaxCycle);
                Gas += 10 * MaxCycle;
                break;
            default:
                break;
        }
        CurrentCycle = 0;
    }

}
