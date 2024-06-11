using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystemManager : MonoBehaviour
{
    IPlayer currentPlayer;
    Turn currentTurn;

    PlayerTurn playerTurn = new PlayerTurn();

    private void Awake()
    {
        
    }

    void Start()
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

        //swap player
        //currentPlayer =;
        currentTurn = newTurn;
        currentTurn.OnStartTurn(currentPlayer);
    }

}
