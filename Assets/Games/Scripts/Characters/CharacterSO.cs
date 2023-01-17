using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Character")]
public class CharacterSO : ScriptableObject
{
    public string characterID;

    public StatsBlock stats;
}
