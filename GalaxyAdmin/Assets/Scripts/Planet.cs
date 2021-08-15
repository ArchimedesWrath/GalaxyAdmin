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
    public int Level = 1;

    public Dictionary<string, float> Materials = new Dictionary<string, float>();

    public bool HarvestReady = false;

    public float CurrentCycle = 0;

    public Planet(string ID, 
        string Type, 
        Sprite Sprite,
        Sprite SecondarySprite,
        float MaxCycle)
    {
        this.ID = ID;
        this.Type = Type;
        this.Sprite = Sprite;
        this.SecondarySprite = SecondarySprite;
        this.MaxCycle = MaxCycle;

        SpawnResources();
    }

    public Planet(PlanetData pd)
    {
        this.ID = pd.ID;
        this.Type = pd.Type;
        this.Sprite = pd.Sprite;
        this.SecondarySprite = pd.Sprite;
        this.MaxCycle = pd.MaxCycle;
        this.Level = pd.Level;

        Materials.Add("ice", pd.Ice);
        Materials.Add("water", pd.Water);
        Materials.Add("gas", pd.Gas);
        Materials.Add("stone", pd.Stone);
        Materials.Add("metal", pd.Metal);
        Materials.Add("life", pd.Life);

    }

    private void SpawnResources()
    {
        Materials.Add("ice", Type == "ice" ? 100 : Type == "water" ? 50 : 0);
        Materials.Add("water", Type == "water" ? 100 : Type == "ice" ? 50 : 0);
        Materials.Add("gas", Type == "gas" ? 100 : Type == "life" ? 50 : 0);
        Materials.Add("stone", Type == "stone" ? 100 : Type == "gas" ? 50 : 0);
        Materials.Add("metal", Type == "metal" ? 50 : Type == "ice" ? 50 : 0);
        Materials.Add("life", Type == "life" ? 20 : 0);
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
        List<string> keys = new List<string>(Materials.Keys);
        foreach(string key in keys)
        {
            if (key.Equals(Type))
            {
                Materials[key] = Materials[key] + 10 + (10 * MaxCycle);
            }
        }
        HarvestReady = true;
        CurrentCycle = 0;
    }

    public void AddMaterial(string mat, float value)
    {
        List<string> keys = new List<string>(Materials.Keys);
        foreach(string key in keys)
        {
            if (key.Equals(mat))
            {
                Materials[key] = Materials[key] + Mathf.Abs(value);
            }
        }
    }

    // TODO: Expand this to add events based on harvest amounts
    public float Harvest(string mat, float value)
    {
        List<string> keys = new List<string>(Materials.Keys);
        foreach (string key in keys)
        {
            if (key.Equals(mat))
            {
                float starting = Materials[key];
                float ending = Mathf.Clamp(starting - value, 0, 99999);
                Materials[key] = starting - ending;
                return starting - ending;
            }
        }
        return 0;
    }

    public bool MaterialCheck(string mat)
    {
        foreach (KeyValuePair<string, float> kvp in Materials)
        {
            if (kvp.Key.Equals(mat))
            {
                return Materials[mat] > 0;
            }
        }
        return false;
    }
}
