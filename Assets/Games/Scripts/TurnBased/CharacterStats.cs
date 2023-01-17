using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatsBlock
{
    public int maxHP;
    public int attack;
    public int defense;
    public int mattack;
    public int mdefense;
    public int speed;
}

public class CharacterStats : MonoBehaviour
{
    public string characterName;

    public int currentHP;

    public StatsBlock stats;

    public void SetStats(StatsBlock _stats)
    {
        stats = _stats;
    }

    public int CalculateAttack()
    {
        return stats.attack;
    }
}
