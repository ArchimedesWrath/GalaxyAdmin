using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour
{
    public Terminal term;
    public Text planetID;

    private void FixedUpdate()
    {
        planetID.text = term.Focus;
    }
}
