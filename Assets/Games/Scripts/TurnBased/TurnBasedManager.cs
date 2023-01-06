using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TurnBasedManager : MonoBehaviour
{
    public const float HUDToggleSpeed = 1.0f;

    private static TurnBasedManager instance = null;
    public static TurnBasedManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject newGameObject = new GameObject();
                instance = newGameObject.AddComponent<TurnBasedManager>();
            }

            return instance;
        }
    }

    public List<BaseTurn> charactersList = new List<BaseTurn>();
    public Queue<ITurn> turnQueue = new Queue<ITurn>();
    ITurn currentState = null;

    public IEnumerable<ITurn> availableCharacters => charactersList.Where(c => c.HP != 0);

    public RectTransform inputPanel;
    public RectTransform statusPanel;
    [SerializeField]
    private TextMeshProUGUI infoTxt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inputPanel.gameObject.SetActive(false);
        HideInput();
        SortQueue();
        NextTurn();
    }

    public void ShowInput()
    {
        inputPanel.gameObject.SetActive(true);
        inputPanel.gameObject.transform.DOScaleY(1, HUDToggleSpeed);
    }

    public void HideInput()
    {
        inputPanel.gameObject.transform.DOScaleY(0, HUDToggleSpeed).OnComplete(() =>
        {
            inputPanel.gameObject.SetActive(false);
            statusPanel.DOScaleX(0, 0.1f);
        });
    }

    public void SortQueue()
    {
        turnQueue = new Queue<ITurn>(availableCharacters.OrderByDescending(c => c.Speed));
    }

    public void SetInfoTxt(string txt)
    {
        infoTxt.SetText(txt);
    }

    public void ShowStatus()
    {
        statusPanel.DOScaleX(1, HUDToggleSpeed);
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        HideInput();
        currentState?.ApplyStatusToTarget(statusEffect);
    }

    public void ApplyStatusEffectParalysed()
    {
        ApplyStatusEffect(StatusEffect.Paralysed);
    }

    public void ApplyStatusEffectPoisoned()
    {
        ApplyStatusEffect(StatusEffect.Poisoned);
    }

    public void HideStatus()
    {
        statusPanel.DOScaleX(0, HUDToggleSpeed);
    }

    public void Attack()
    {
        HideInput();
        currentState?.Attack();
    }

    public void RemoveFromQueue(ITurn target)
    {
        List<ITurn> turns = turnQueue.ToList();
        turns.Remove(target);
        turnQueue = new Queue<ITurn>(turns);
    }

    public void NextTurn()
    {
        bool hasPlayerWon = availableCharacters.Where(c => c is EnemyTurn).Count() == 0;
        bool hasAIWon = availableCharacters.Where(c => c is PlayerTurn).Count() == 0;

        if (hasPlayerWon || hasAIWon)
        {
            SetInfoTxt($"Game Over! You {(hasPlayerWon ? "Won" : "Lost")}");
            return;
        }

        ITurn turn = turnQueue.Dequeue();

        if (turnQueue.Count == 0)
        {
            SortQueue();
        }

        if (turn.HP > 0)
        {
            SetState(turn);
        }
        else
        {
            NextTurn();
        }
    }

    private void SetState(ITurn nextTurn)
    {
        Debug.Log($"Changing State {nextTurn}");

        currentState?.OnEnd();
        currentState = nextTurn;
        currentState?.OnStart();
    }

    private void Update()
    {
        currentState?.OnUpdate();
    }
}
