using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Terminal : MonoBehaviour
{
    public TMP_InputField InputField;
    public TMP_InputField TerminalOutput;

    public List<Commands> CommandList = new List<Commands>();

    public List<string> CommandHistory = new List<string>();
    public int commandIndex = 0;

    public Dictionary<string, Planet> AllPlanets = new Dictionary<string, Planet>();
    public Dictionary<string, Planet> SpaceObjects = new Dictionary<string, Planet>();

    public Dictionary<string, string> Syntax = new Dictionary<string, string>();

    public string Focus;

    public string[] materialTypes = new string[] { "ice", "water", "metal", "stone", "gas", "life" };

    public Player Player;
    public Shop Shop;

    private void Start()
    {
        // Initialize Player
        Player = new Player();
        AllPlanets = LoadAllPlanets();
        Syntax = GenerateSyntax();

        // Get all commands
        Commands[] allCommands = Resources.LoadAll("Commands", typeof(Commands)).Cast<Commands>().ToArray();
        foreach (Commands command in allCommands)
        {
            CommandList.Add(command);
        }

        LoadPlanets();

        // Initialize shop
        Shop = new Shop(this);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {

            string input = InputField.text;
            InputField.text = "";

            ProcessInput(input);
            commandIndex = CommandHistory.Count - 1;
            InputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CommandHistory.Count > 0)
            {
                InputField.text = CommandHistory[Mathf.Clamp(commandIndex, 0, CommandHistory.Count - 1)];
                commandIndex = commandIndex - 1 >= 0 ? commandIndex - 1 : CommandHistory.Count - 1;
                InputField.ActivateInputField();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CommandHistory.Count > 0)
            {
                InputField.text = CommandHistory[Mathf.Clamp(commandIndex, 0, CommandHistory.Count - 1)];
                commandIndex = commandIndex + 1 <= CommandHistory.Count - 1 ? commandIndex + 1 : 0;
                InputField.ActivateInputField();
            }
        }
    }


    private void LoadPlanets()
    {
        PlanetData[] p = Resources.LoadAll("Planets", typeof(PlanetData)).Cast<PlanetData>().ToArray();
        Planet planet = new Planet(p[0]);
        SpaceObjects.Add(planet.ID, planet);
        var first = SpaceObjects.First();
        Focus = first.Key;
    }

    private Dictionary<string, string> GenerateSyntax()
    {
        /*
         * ice - cyan - #00ccff
         * water - blue - #0066ff
         * gas - red - #ff0066
         * stone - orange - #ff9900
         * metal - silver - #9975bd 
         * life - golden - #f0f075
         */
        Dictionary<string, string> syntax = new Dictionary<string, string>();
        syntax.Add("ice", "<#379b37>ice</color>");
        syntax.Add("water", "<#379b37>water</color>");
        syntax.Add("gas", "<#379b37>gas</color>");
        syntax.Add("stone", "<#379b37>stone</color>");
        syntax.Add("metal", "<#379b37>metal</color>");
        syntax.Add("life", "<#379b37>life</color>");
        return syntax;
    }

    private Dictionary<string, Planet> LoadAllPlanets()
    {
        Dictionary<string, Planet> planets = new Dictionary<string, Planet>();
        PlanetData[] allPlanetData = Resources.LoadAll("Planets", typeof(PlanetData)).Cast<PlanetData>().ToArray();
        foreach (PlanetData p in allPlanetData)
        {
            Planet planet = new Planet(p);
            planets.Add(planet.ID, planet);
        }
        return planets;
    }

    private void ProcessInput(string text)
    {
        string[] tokens = text.Split(' ');
        switch (tokens[0].ToLower())
        {
            case "buy":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Buy(tokens);
                break;
            case "wait":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Wait();
                break;
            case "harvest":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Harvest(tokens);
                break;
            case "focus":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                ChangeFocus(tokens);
                break;
            case "clear":
                TerminalOutput.text = "";
                CommandHistory.Add(text);
                break;
            case "help":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Help();
                break;
            case "peek":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Peek(tokens);
                break;
            case "planets":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                ListPlanets();
                break;
            case "shop":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                ShowShop();
                break;
            case "add":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Add(tokens);
                break;
            case "inventory":
                OutputToTerminal("> " + text);
                CommandHistory.Add(text);
                Inventory();
                break;
            default:
                OutputToTerminal("> " + text);
                OutputToTerminal($"\'{tokens[0].ToLower()}\' is not a command.\n\tTo view a list of commands type \'help\'");
                break;
        }
    }

    public void OutputToTerminal(string text)
    {
        foreach(string s in Syntax.Keys)
        {
            text = text.Replace(s, Syntax[s]);
        }

        TerminalOutput.text = TerminalOutput.text + text + "\n";
        TMP_Text textComponent = TerminalOutput.textComponent;
        RectTransform textViewport = TerminalOutput.textViewport;
        // TODO: Make this exactly how I want it.
        textComponent.rectTransform.anchoredPosition = new Vector2(
            textComponent.rectTransform.anchoredPosition.x,
            (textComponent.preferredHeight - textViewport.rect.height) * 1
            );
    }

    private void GeneratePlanets(int planetCount)
    {
        // TODO : Update this function to actually work
        PlanetData[] allPlanetData = Resources.LoadAll("Planets", typeof(PlanetData)).Cast<PlanetData>().ToArray();
        PlanetData randomPlanet = allPlanetData[Random.Range(0, allPlanetData.Length)];
        Planet planet = new Planet(randomPlanet.ID,
            randomPlanet.Type,
            randomPlanet.Sprite,
            randomPlanet.SecondarySprite,
            randomPlanet.MaxCycle);
        SpaceObjects.Add(planet.ID, planet);
    }


    private void Buy(string[] tokens)
    {
        if (tokens.Length == 2) // buy powerup 
        {
            string itemName = tokens[1].ToLower();
            string[] keys = Shop.Items.Keys.Select(s => s.ToLower()).ToArray();
            if (keys.Contains(itemName))
            {
                float cost = Shop.Items[itemName].cost;
                if (Shop.Items[itemName].amount > 0)
                {
                    if (Player.CanPurchase(cost))
                    {
                        switch (Shop.Items[itemName].type) {
                            case "material":
                                Player.BuyMat(itemName, 1, cost);
                                Shop.Sell(itemName, 1);
                                break;
                            case "planet":
                                Planet p = Player.BuyPlanet(AllPlanets[itemName.ToUpper()], cost);
                                Shop.Sell(itemName, 1);
                                SpaceObjects.Add(p.ID, p);
                                break;
                            case "upgrade":
                                break;
                            default:
                                break;
                        }
                        OutputToTerminal($"You bought 1 {itemName} for {cost} credits");
                    } else
                    {
                        OutputToTerminal("Not enough credits to purchase");
                    }
                } else
                {
                    OutputToTerminal("That item is no longer in stock");
                }
            } else
            {
                OutputToTerminal($"Shop does not contain item \'{itemName}\'");
            }
        } else if (tokens.Length == 3) // buy 100 material
        {
            BuyMultiple(tokens);
        } else
        {
            OutputToTerminal($"Command does not match format buy <item> or buy <amount> <item>");
        }
    }

    public void BuyMultiple(string[] tokens)
    {
        string itemName = tokens[2].ToLower();
        string[] keys = Shop.Items.Keys.Select(s => s.ToLower()).ToArray();

        float value;
        if (!float.TryParse(tokens[1], out value))
        {
            OutputToTerminal($"{tokens[1]} is not a number");
            return;
        } else if (value <= 0)
        {
            OutputToTerminal("Cannot buy 0 or less of an item");
            return;
        }

        if (keys.Contains(itemName))
        {
            float cost = Shop.Items[itemName].cost;
            if (Shop.Items[itemName].amount >= value)
            {
                if (Player.CanPurchase(cost))
                {
                    switch (Shop.Items[itemName].type)
                    {
                        case "material":
                            Player.BuyMat(itemName, value, cost);
                            Shop.Sell(itemName, value);
                            break;
                        case "planet":
                            Planet p = Player.BuyPlanet(AllPlanets[itemName.ToUpper()], cost);
                            Shop.Sell(itemName, value);
                            SpaceObjects.Add(p.ID, p);
                            break;
                        case "upgrade":
                            break;
                        default:
                            break;
                    }
                    OutputToTerminal($"You bought {value} {itemName} for {cost} credits");
                }
                else
                {
                    OutputToTerminal("Not enough credits to purchase");
                }
            }
            else
            {
                OutputToTerminal($"The shop doesn't have {value} {itemName}");
            }
        }
        else
        {
            OutputToTerminal($"Shop does not contain item \'{itemName}\'");
        }
    }

    private void ShowShop()
    {
        foreach(Item i in Shop.Items.Values)
        {
            OutputToTerminal($"| {i.ID} | {i.type} | {i.amount} | {i.cost} |");
        }
    }

    private void Peek(string[] tokens)
    {
        if (tokens.Length >= 2)
        {
            string planetID = tokens[1].ToUpper();
            if (SpaceObjects.ContainsKey(planetID))
            {
                Planet planet = SpaceObjects[planetID];
                OutputToTerminal("Name: " + planet.ID);
                OutputToTerminal($"type: {planet.Type}");
                foreach(KeyValuePair<string, float> kvp in planet.Materials)
                {
                    OutputToTerminal($"{kvp.Key}: {kvp.Value}");
                }
                OutputToTerminal(planet.HarvestReady ? "ready to harvest" : "not ready to harvest");
                OutputToTerminal($"cycle: {planet.CurrentCycle}/{planet.MaxCycle}");
            } else
            {
                OutputToTerminal($"Planet \'{planetID}\' does not exist in your system");
            }
        } else
        {
            OutputToTerminal($"Command does not match format peek <planet>");
        }
    }

    private void Inventory()
    {
        OutputToTerminal($"Credits: {Player.Credits}");
        foreach (KeyValuePair<string, float> kvp in Player.Materials)
        {
            OutputToTerminal($"{kvp.Key}: {kvp.Value}");
        }
    }

    private void Harvest(string[] tokens)
    {
        if (tokens.Length < 3)
        {
            OutputToTerminal("Wrong number of arguements for harvest command");
        } else
        {
            string matType = tokens[1].ToLower();
            if (materialTypes.Contains(matType))
            {
                string planet = tokens[2].ToUpper();
                if (SpaceObjects.ContainsKey(planet))
                {
                    Planet p = SpaceObjects[planet];
                    if (p.HarvestReady)
                    {
                        if (p.MaterialCheck(matType))
                        {
                            float amount = p.Harvest(matType, Player.HarvestingProf * 10);
                            Player.AddMaterials(matType, amount);
                            OutputToTerminal($"Harvested {amount} {matType} from {planet}");
                        }
                        else
                        {
                            OutputToTerminal($"{planet} has no {matType} to harvest");
                        }
                    }
                    else
                    {
                        OutputToTerminal($"{planet} is not ready to harvest");
                    }
                } else
                {
                    OutputToTerminal($"Planet \'{planet}\' does not exist in your system");
                }
            } else
            {
                OutputToTerminal($"\'{matType}\' is not a valid material type");
            }
        }
    }

    private void Add(string[] tokens)
    {
        if (tokens.Length >= 4)
        {
            string mat = tokens[1].ToLower();
            if (materialTypes.Contains(mat))
            {
                string planetID = tokens[2].ToUpper();
                if (SpaceObjects.ContainsKey(planetID))
                {
                    float value;
                    if (float.TryParse(tokens[3], out value))
                    {
                        if (Player.CheckMaterials(mat, value))
                        {
                            Player.RemoveMaterials(mat, value);
                            SpaceObjects[planetID].AddMaterial(mat, value);
                            OutputToTerminal($"{value} {mat} added to {planetID}");
                        } else
                        {
                            OutputToTerminal($"You do not have {value} {mat}");
                        }
                    } else
                    {
                        OutputToTerminal($"\'{tokens[3]}\' is not a number");
                    }
                } else
                {
                    OutputToTerminal($"Planet \'{planetID}\' does not exist in your system");
                }
            } else
            {
                OutputToTerminal($"\'{mat}\' is not a valid material type");
            }
        } else
        {
            OutputToTerminal("Command does not match formatt add <material> <planet> <value>");
        }
    }

    private void Wait()
    {
        List<string> keys = new List<string>(SpaceObjects.Keys);
        foreach(string key in keys)
        {
            SpaceObjects[key].WaitCycle();
        }
        OutputToTerminal("Waiting an orbit cycle...");
    }

    private void ChangeFocus(string[] tokens)
    {
        string planet = tokens[1].ToUpper();
        if (SpaceObjects.ContainsKey(planet))
        {
            Focus = planet;
            OutputToTerminal($"Now viewing {planet}");
        } else
        {
            OutputToTerminal($"Planet \'{planet}\' does not exist in your system");
        }
    }

    private void Help()
    {
        OutputToTerminal("Help System");
        foreach(Commands c in CommandList)
        {
            OutputToTerminal($"{c.Name} - {c.Description}");
            if (!c.Format.Equals("?")) OutputToTerminal($"{c.Format}");
        }
    }

    private void ListPlanets()
    {
        OutputToTerminal("Current planets: ");
        foreach(string s in SpaceObjects.Keys)
        {
            OutputToTerminal(s);
        }
    }
}
