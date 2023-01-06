using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTurn : BaseTurn
{
    public override void OnStart()
    {
        Attack();
    }

    public override void OnUpdate()
    {

    }

    public override void OnEnd()
    {

    }

    public override void Attack()
    {
        var targets = TurnBasedManager.Instance.availableCharacters.Where(c => c is PlayerTurn).ToList();
        if (targets.Count > 0)
        {
            ITurn target = targets[Random.Range(0, targets.Count)];
            target.TakeDamage(Random.Range(stats.attack/2, stats.attack));
        }
    }
}
