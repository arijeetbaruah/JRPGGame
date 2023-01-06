using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurn : BaseTurn
{
    public override void OnStart()
    {
        
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public override void OnEnd()
    {
        
    }

    public override void Attack()
    {
        List<ITurn> targets = TurnBasedManager.Instance.availableCharacters.Where(c => c is EnemyTurn).ToList();
        if (targets.Count > 0 )
        {
            ITurn target = targets[Random.Range(0, targets.Count)];
            target.TakeDamage(stats.attack);
        }
    }
}
