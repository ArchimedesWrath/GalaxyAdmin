using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Terminal : MonoBehaviour
{
    public TMP_InputField InputField;
    public TMP_InputField TerminalOutput;

    public List<Commands> CommandList = new List<Commands>();

    public Dictionary<string, Planet> SpaceObjects = new Dictionary<string, Planet>();
    public string Focus;

    public Player Player;

    private void Start()
    {
        // Get all commands
        Commands[] allCommands = Resources.LoadAll("Commands", typeof(Commands)).Cast<Commands>().ToArray();
        foreach (Commands command in allCommands)
        {
            Debug.Log($"Adding command: {command.Name}");
            CommandList.Add(command);
        }

        PlanetData[] allPlanetData = Resources.LoadAll("Planets", typeof(PlanetData)).Cast<PlanetData>().ToArray();
        foreach(PlanetData p in allPlanetData)
        {
            Planet planet = new Planet(p.ID,
            p.Type,
            p.Ice,
            p.Water,
            p.Gas,
            p.Stone,
            p.Metal,
            p.Life,
            p.Sprite,
            p.SecondarySprite,
            p.MaxCycle);
            SpaceObjects.Add(planet.ID, planet);
            Debug.Log($"Added {planet.ID} to SpaceObjects.");
        }
        var first = SpaceObjects.First();
        Focus = first.Key;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {

            string input = InputField.text;
            InputField.text = "";

            ProcessInput(input);
        }
    }

    private void ProcessInput(string text)
    {
        string[] tokens = text.Split(' ');

        /*
        ice
        inv
        planets
        stone
        gas
        power
        life
        buy 
        sell
         */
        switch(tokens[0].ToLower())
        {
            case "focus":
                OutputToTerminal("> " + text);
                ChangeFocus(tokens);
                break;
            case "clear":
                TerminalOutput.text = "";
                break;
            case "help":
                OutputToTerminal("> " + text);
                Help();
                break;
            case "planets":
                OutputToTerminal("> " + text);
                ListPlanets();
                break;
            case "water":
                OutputToTerminal("> " + text.Replace("water", "<#3399ff>water</color>"));
                AddWater(tokens);
                break;
            default:
                OutputToTerminal("> " + text);
                OutputToTerminal($"\'{tokens[0].ToLower()}\' is not a command.\n\tTo view a list of commands type \'help\'");
                break;
        }
    }

    public void OutputToTerminal(string text)
    {
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
            randomPlanet.Ice,
            randomPlanet.Water,
            randomPlanet.Gas,
            randomPlanet.Stone,
            randomPlanet.Metal,
            randomPlanet.Life,
            randomPlanet.Sprite,
            randomPlanet.SecondarySprite,
            randomPlanet.MaxCycle);
        SpaceObjects.Add(planet.ID, planet);
        Debug.Log($"Added {planet.ID} to SpaceObjects.");
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
            Debug.Log(c.Name);
            OutputToTerminal($"{c.Name} - {c.Description}");
            if (!c.Format.Equals("?")) OutputToTerminal($"\t{c.Format}");
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

    private void AddWater(string[] tokens)
    {
        // First check that there are 3 tokens
        if (tokens.Length == 3)
        {
            // Then check that the planet exists
            if (SpaceObjects.ContainsKey(tokens[1].ToUpper()))
            {
                // Then check if the value is a number
                float value;
                if (float.TryParse(tokens[2], out value))
                {
                    // Then check if there is enough water in the player's inventory
                    if (value <= Player.Water)
                    {
                        if (value > 0)
                        {
                            // Take water from player and give to planet
                            Player.Water -= value;
                            SpaceObjects[tokens[1].ToUpper()].Water = value;
                            OutputToTerminal($"{value} <#3399ff>water</color> was given to {tokens[1].ToUpper()}");
                        } else
                        {
                            OutputToTerminal($"Cannot give 0 or less resources");
                        }
                    } else
                    {
                        OutputToTerminal("Not enough <#3399ff>water</color> in inventory.\nUse <#00ccff>inv</color> to check your inventory.");
                    }
                }
                else
                {
                    OutputToTerminal($"<#ff0000>Error</color>: The entered value '{tokens[2]}' is not a number");
                }
            }
            else
            {
                // If the planet doesn't exist then throw an error.
                OutputToTerminal($"Your list of planets does not include <#ff0000>'{tokens[1]}'</color>");
            }
        } else 
        {
            // If there aren't 3 tokens then throw an error
            OutputToTerminal($"Your <#3399ff>water</color> command does not match format of:\n\t<#3399ff>water</color> <planet> <value>");
        } 
    }

}
