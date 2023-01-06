using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTurn : MonoBehaviour, ITurn
{
    [SerializeField]
    protected CharacterStats stats;
    [SerializeField]
    protected CinemachineVirtualCamera vcam;
    [SerializeField]
    protected OverheadPopup overheadPopup;
    [SerializeField]
    protected Image hpBar;


    [SerializeField]
    protected Image paralysiedImage;
    [SerializeField]
    protected Image poisonImage;

    protected int paralysisTimer = 0;

    protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    public int HP => stats.currentHP;

    public int Speed => stats.speed;

    public abstract void Attack();

    public abstract void OnEnd();

    public abstract void OnStart();

    public abstract void OnUpdate();
    public abstract void ApplyStatusToTarget(StatusEffect statusEffect);


    public void AddStatusEffect(StatusEffect _statusEffect)
    {
        StartCoroutine(OnStatusEffect(_statusEffect));
    }

    public void RemoveStatusEffect(StatusEffect _statusEffect)
    {
        statusEffects.Remove(_statusEffect);
    }

    protected IEnumerator OnStatusEffect(StatusEffect _statusEffect)
    {
        FocusCamera(true);
        yield return new WaitForSeconds(2);
        statusEffects.Add(_statusEffect);
        switch (_statusEffect)
        {
            case StatusEffect.Paralysed:
                paralysiedImage.gameObject.SetActive(true);
                break;

            case StatusEffect.Poisoned:
                poisonImage.gameObject.SetActive(true);
                break;
        }

        TurnBasedManager.Instance.SetInfoTxt($"{stats.characterName} is {_statusEffect.ToString()}");
        yield return new WaitForSeconds(2);
        FocusCamera(false);
        yield return new WaitForSeconds(2);
        TurnBasedManager.Instance.NextTurn();
    }

    protected virtual IEnumerator StartTurnStartCorrutine()
    {
        TurnBasedManager.Instance.SetInfoTxt($"{stats.characterName} Turn");
        yield return new WaitForSeconds(2);
        if (statusEffects.Contains(StatusEffect.Poisoned))
        {
            TurnBasedManager.Instance.SetInfoTxt($"{stats.characterName} is Poisoned");
            yield return new WaitForSeconds(2);
            TakeDamage(2);
            yield return new WaitForSeconds(2);
        }
    }

    protected IEnumerator OnParalysed()
    {
        //TurnBasedManager.Instance.();
        yield return new WaitForSeconds(3);
        TurnBasedManager.Instance.NextTurn();
    }

    public void TakeDamage(int dmg)
    {
        StartCoroutine(TakeDamageCorrutine(dmg));
    }

    public void FocusCamera(bool active)
    {
        vcam.Priority = active ? 100 : 1;
    }

    public void UpdateHP()
    {
        float targetFill = (float)stats.currentHP / (float)stats.maxHP;

        hpBar.fillAmount = targetFill;
    }

    protected IEnumerator TakeDamageCorrutine(int dmg)
    {
        FocusCamera(true);
        TurnBasedManager.Instance.SetInfoTxt(" ");
        yield return new WaitForSeconds(3);

        TurnBasedManager.Instance.SetInfoTxt($"{stats.characterName} took {dmg} damage");

        var popup = Instantiate(overheadPopup, transform);
        popup.ShowText(dmg.ToString());
        stats.currentHP = Mathf.Max(0, stats.currentHP - dmg);
        Debug.Log(stats.currentHP);
        UpdateHP();

        yield return new WaitForSeconds(3);
        TurnBasedManager.Instance.SetInfoTxt(" ");
        FocusCamera(false);

        if (stats.currentHP == 0)
        {
            TurnBasedManager.Instance.SetInfoTxt($"{stats.characterName} is eleminated");
            transform.DORotate(new Vector3(90, 0, 0), 1);
            hpBar.transform.parent.parent.gameObject.SetActive(false);
            TurnBasedManager.Instance.RemoveFromQueue(this);
        }
        yield return new WaitForSeconds(3);

        TurnBasedManager.Instance.NextTurn();
    }
}
