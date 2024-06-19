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
    public IPlayer currentPlayer;
    public Turn currentTurn;

    //declaration of possible turns
    PlayerTurn playerTurn = new PlayerTurn();

    //declaration of events
    public event Action<TurnSystemManager, Turn, Turn> OnChangedTurnEvent;


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
        currentPlayer = HumanPlayer;
        ChangeTurn(playerTurn);

        //TEMP TESTING
        //Invoke(nameof(ChangeTurnTest), 5f);
    }

    //TEMP
    private void ChangeTurnTest()
    {
        ChangeTurn(playerTurn);
    }


    void Update()
    {
        if(currentTurn != null)
        {
            currentTurn.OnUpdateTurn(currentPlayer);
        }
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
}
