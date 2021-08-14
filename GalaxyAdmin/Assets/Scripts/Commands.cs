using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "ScriptableObjects/Command")]
public class Commands : ScriptableObject
{
    public string Name;
    public string Short;
    public string Format;
    public string Description;
}
