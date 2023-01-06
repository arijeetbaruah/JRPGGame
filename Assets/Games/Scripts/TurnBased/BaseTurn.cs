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

    public int HP => stats.currentHP;

    public int Speed => stats.speed;

    public abstract void Attack();

    public abstract void OnEnd();

    public abstract void OnStart();

    public abstract void OnUpdate();

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
        yield return new WaitForSeconds(3);
        var popup = Instantiate(overheadPopup, transform);
        popup.ShowText(dmg.ToString());
        stats.currentHP = Mathf.Max(0, stats.currentHP - dmg);
        Debug.Log(stats.currentHP);
        UpdateHP();

        yield return new WaitForSeconds(3);
        FocusCamera(false);
        
        if (stats.currentHP == 0)
        {
            
        }
        yield return new WaitForSeconds(3);

        TurnBasedManager.Instance.NextTurn();
    }
}
