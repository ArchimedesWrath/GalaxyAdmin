using System;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public float Credits;
    public int HarvestingProf = 1;
    public Dictionary<string, float> Materials = new Dictionary<string, float>();

    public Player()
    {
        Materials.Add("ice", 9999);
        Materials.Add("water", 9999);
        Materials.Add("gas", 9999);
        Materials.Add("stone", 9999);
        Materials.Add("metal", 9999);
        Materials.Add("life", 9999);
        Credits = 777777;
    }

    public bool CheckMaterials(string mat, float value)
    {
        foreach(string key in Materials.Keys)
        {
            if (key.Equals(mat))
            {
                return Materials[key] >= value ? true : false;
            }
        }
        return false;
    }

    public bool RemoveMaterials(string mat, float value)
    {
        List<string> keys = new List<string>(Materials.Keys);
        foreach (string key in keys)
        {
            if (key.Equals(mat))
            {
                float remove = Materials[key] - Mathf.Abs(value);
                if (remove < 0)
                {
                    return false;
                } else
                {
                    Materials[key] -= Mathf.Abs(value);
                    return true;
                }
            }
        }
        return false;
    }

    public void AddMaterials(string mat, float value)
    {
        List<string> keys = new List<string>(Materials.Keys);
        foreach (string key in keys)
        {
            if (key.Equals(mat))
            {
                Materials[key] = Materials[key] + Mathf.Abs(value);
            }
        }
    }

    public bool CanPurchase(float cost)
    {
        return Credits >= cost;
    }

    public void BuyMat(string mat, float amount, float cost)
    {
        Credits -= cost * amount;
        AddMaterials(mat, amount);
    }

    public Planet BuyPlanet(Planet p, float cost)
    {
        Credits -= cost;
        return p;
    }

    public void BuyUpgrade(string p, float cost)
    {
        Credits -= cost;
        // TODO: Implement Upgrade
    }
}
