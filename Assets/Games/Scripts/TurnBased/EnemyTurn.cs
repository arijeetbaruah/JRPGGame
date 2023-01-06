using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTurn : BaseTurn
{
    public override void OnStart()
    {
        TurnBasedManager.Instance.inputPanel.gameObject.SetActive(false);
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

    public override void ApplyStatusToTarget(StatusEffect statusEffect)
    {
        var targets = TurnBasedManager.Instance.availableCharacters.Where(c => c is PlayerTurn).ToList();
        if (targets.Count > 0)
        {
            ITurn target = targets[Random.Range(0, targets.Count)];
            target.AddStatusEffect(statusEffect);
        }
    }
}
