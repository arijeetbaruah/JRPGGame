using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnBasedManager : MonoBehaviour
{
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
        SortQueue();
        NextTurn();
    }

    public void SortQueue()
    {
        turnQueue = new Queue<ITurn>(availableCharacters.OrderByDescending(c => c.Speed));
    }

    public void NextTurn()
    {
        bool hasPlayerWon = availableCharacters.Where(c => c is EnemyTurn).Count() == 0;
        bool hasAIWon = availableCharacters.Where(c => c is PlayerTurn).Count() == 0;

        if (hasPlayerWon || hasAIWon)
        {
            Debug.Log("Game Over");
            return;
        }

        ITurn turn = turnQueue.Dequeue();
        SetState(turn);

        if (turnQueue.Count == 0)
        {
            SortQueue();
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
