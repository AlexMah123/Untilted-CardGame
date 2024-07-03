using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystemManager : MonoBehaviour
{
    public static TurnSystemManager Instance;

    private Player HumanPlayer { get => GameManager.Instance.humanPlayer; }
    private Player AiPlayer { get => GameManager.Instance.humanPlayer; }

    //Current values of the game.
    public Player currentPlayer;
    public Turn currentTurn;

    //declaration of possible turns
    PlayerTurn playerTurn = new PlayerTurn();

    //declaration of events
    public event Action<TurnSystemManager, Turn, Turn> OnChangedTurnEvent;
    private bool isStartNewTurnEventBinded = false;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            BindStartNewTurnEvent();
        }
    }

    private void OnDisable()
    {
        if(isStartNewTurnEventBinded)
        {
            UnbindStartNewTurnEvent();
        }
    }


    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        //To avoid racing condition
        if (!isStartNewTurnEventBinded)
        {
            BindStartNewTurnEvent();
        }

        currentPlayer = HumanPlayer;
        ChangeTurn(playerTurn);
    }

    void Update()
    {
        if (currentTurn != null)
        {
            currentTurn.OnUpdateTurn(currentPlayer);
        }
    }

    public void HandleStartNewTurn()
    {
        ChangeTurn(playerTurn);
    }

    public void ChangeTurn(Turn newTurn)
    {
        if(currentTurn != null)
        {
            currentTurn.OnEndTurn(currentPlayer);
        }

        //#TODO: Should Swap Player?

        //broadcast event, primarily binded to PlayerHandUIManager
        OnChangedTurnEvent?.Invoke(this, currentTurn, newTurn);
        currentTurn = newTurn;

        currentTurn.OnStartTurn(this, currentPlayer);
    }

    #region Bind StartNewTurnEvent

    private void BindStartNewTurnEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartNewTurnEvent += HandleStartNewTurn;
            isStartNewTurnEventBinded = true;
        }
    }

    private void UnbindStartNewTurnEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartNewTurnEvent -= HandleStartNewTurn;
            isStartNewTurnEventBinded = false;
        }
    }
    #endregion
}
