using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystemManager : MonoBehaviour
{
    public static TurnSystemManager Instance;

    public Player humanPlayer;
    public Player aiPlayer;

    public IPlayer currentPlayer;
    public Turn currentTurn;

    PlayerTurn playerTurn = new PlayerTurn();

    public event Action<TurnSystemManager, Turn, Turn> OnChangedTurn;

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
        currentPlayer = humanPlayer;
        ChangePhase(playerTurn);

        //TEMP TESTING
        //Invoke(nameof(ChangePhaseTest), 5f);
    }

    //TEMP
    private void ChangePhaseTest()
    {
        ChangePhase(playerTurn);
    }


    void Update()
    {
        if(currentTurn != null)
        {
            currentTurn.OnUpdateTurn(currentPlayer);
        }
    }

    public void ChangePhase(Turn newTurn)
    {
        if(currentTurn != null)
        {
            currentTurn.OnEndTurn(currentPlayer);
        }

        //swap player?
        OnChangedTurn?.Invoke(this, currentTurn, newTurn);
        currentTurn = newTurn;

        currentTurn.OnStartTurn(this, currentPlayer);
    }

}
