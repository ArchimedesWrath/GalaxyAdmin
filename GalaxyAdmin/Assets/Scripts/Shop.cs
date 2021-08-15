using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Shop 
{
    public Dictionary<string, Item> Items = new Dictionary<string, Item>();
    public int DayCounter = 0;
    public int DayReset = 10;

    public Shop(Terminal terminal)
    {
        List<PlanetData> allPlanetData = Resources.LoadAll("Planets", typeof(PlanetData)).Cast<PlanetData>().ToList();
        List<PlanetData> planetDataList = new List<PlanetData>(allPlanetData);
        foreach(string s in terminal.SpaceObjects.Keys)
        {
            foreach(PlanetData pd in planetDataList)
            {
                if (pd.ID.Equals(s))
                {
                    allPlanetData.Remove(pd);
                }
            }
        }
        PlanetData randomPlanet = allPlanetData[Random.Range(0, allPlanetData.Count)];
        Item item = new Item(randomPlanet);
        Items.Add(item.ID.ToLower(), item);

        Items.Add("ice", new Item("ice", "material", 100, 10));
        Items.Add("water", new Item("water", "material", 100, 10));
        Items.Add("gas", new Item("gas", "material", 100, 10));
        Items.Add("stone", new Item("stone", "material", 100, 10));
        Items.Add("metal", new Item("metal", "material", 100, 10));
        Items.Add("life", new Item("life", "material", 100, 10));

    }

    public bool WaitCycle()
    {
        DayCounter += 1;
        if (DayCounter >= DayReset)
        {
            UpdateShop();
            DayCounter = 0;
            return true;
        }
        return false;
    }

    public void UpdateShop()
    {
        // Update the shop every 10 cycles
    }

    public void Sell(string itemName, float amount)
    {
        foreach(string s in Items.Keys)
        {
            if (s.Equals(itemName))
            {
                Items[s].amount = Items[s].amount - amount;
            }
        }
    }
}

public class Item
{
    public string ID;
    public string type;
    public float cost;
    public float amount;

    public Item(string ID, string type, float cost, float amount)
    {
        this.ID = ID;
        this.type = type;
        this.cost = cost;
        this.amount = amount;
    }

    public Item(MaterialData pd)
    {
        this.ID = pd.ID;
        this.type = pd.type;
        this.cost = pd.cost;
        this.amount = pd.amount;
    }

    public Item(PlanetData pd)
    {
        this.ID = pd.ID;
        this.type = "planet";
        this.cost = 1; // TODO: Add some formula for establishing planet cost
        this.amount = 1;
    }

    public Item(UpgradeData pd)
    {
        this.ID = pd.ID;
        this.type = pd.type;
        this.cost = pd.cost;
        this.amount = pd.amount;
    }
}
