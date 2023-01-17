using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurn : BaseTurn
{
    public override void OnStart()
    {
        StartCoroutine(StartTurnStartCorrutine());
    }

    protected override IEnumerator StartTurnStartCorrutine()
    {
        yield return base.StartTurnStartCorrutine();
        yield return new WaitForSeconds(2);

        if (statusEffects.Contains(StatusEffect.Paralysed))
        {
            TurnBasedManager.Instance.SetInfoTxt($"{stats.characterName} is Paraliysd");
            paralysisTimer--;
            if (paralysisTimer <= 0)
            {
                statusEffects.Remove(StatusEffect.Paralysed);
            }
            
            yield return new WaitForSeconds(2);
            TurnBasedManager.Instance.NextTurn();

            yield break;
        }

        TurnBasedManager.Instance.ShowInput();
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
        TurnBasedManager.Instance.HideInput();
    }

    public override void Attack()
    {
        List<ITurn> targets = TurnBasedManager.Instance.availableCharacters.Where(c => c is EnemyTurn).ToList();
        if (targets.Count > 0 )
        {
            ITurn target = targets[Random.Range(0, targets.Count)];
            float dmg = (float) stats.CalculateAttack() / (float) target.CalculateDefense();
            target.TakeDamage(Mathf.CeilToInt(dmg));
        }
    }

    public override void ApplyStatusToTarget(StatusEffect statusEffect)
    {
        List<ITurn> targets = TurnBasedManager.Instance.availableCharacters.Where(c => c is EnemyTurn).ToList();
        if (targets.Count > 0)
        {
            ITurn target = targets[Random.Range(0, targets.Count)];
            target.AddStatusEffect(statusEffect);
        }
    }
}
