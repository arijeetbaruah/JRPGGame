using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Character")]
public class CharacterSO : ScriptableObject
{
    public string characterID;

    [Button]
    public void FetchData()
    {
        PlayfabEconomy.GetItemInfo(characterID);
    }
}
