using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameResult
{
    NONE,
    WIN,
    LOSE,
    DRAW
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Configs")]
    public Player humanPlayer;
    public Player aiPlayer;

    //declaration of events
    public event Action OnClearCardHandEvent;
  
    private bool IsConfirmCardEventBinded = false;
    
    private void OnEnable()
    {
        if (CardConfirmation.Instance != null)
        {
            BindConfirmCardChoiceEvent();
        }
    }
    private void OnDisable()
    {
        if(IsConfirmCardEventBinded)
        {
            UnbindConfirmCardChoiceEvent();
        }
    }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        //To avoid racing condition
        if (!IsConfirmCardEventBinded)
        {
            BindConfirmCardChoiceEvent();
        }
    }

    public GameResult GetGameResult(GameChoice humanPlayerChoice, GameChoice aiPlayerChoice)
    {
        var result = GameResult.NONE;

        if(humanPlayerChoice == aiPlayerChoice)
        {
            result = GameResult.DRAW;
        }
        else if(humanPlayerChoice == GameChoice.ROCK && aiPlayerChoice == GameChoice.SCISSOR||
                humanPlayerChoice == GameChoice.PAPER && aiPlayerChoice == GameChoice.ROCK ||
                humanPlayerChoice == GameChoice.SCISSOR && aiPlayerChoice == GameChoice.PAPER)
        {
            result = GameResult.WIN;
        }
        else
        {
            result = GameResult.LOSE;
        }

        return result;
    }

    public void HandleConfirmCardChoice(CardUI cardUI)
    {
        //#TODO: GetGameResult, and change the turn
        humanPlayer.currentChoice = cardUI.gameChoice;
        Debug.Log($"{humanPlayer.currentChoice} was selected");

        //broadcast event, primarily binded to PlayerHandUIManager
        OnClearCardHandEvent?.Invoke();
    }

    #region Bind CardConfirmation event
    private void BindConfirmCardChoiceEvent()
    {
        if (CardConfirmation.Instance != null)
        {
            CardConfirmation.Instance.OnConfirmCardChoiceEvent += HandleConfirmCardChoice;
            IsConfirmCardEventBinded = true;
        }
    }

    private void UnbindConfirmCardChoiceEvent()
    {
        if (CardConfirmation.Instance != null)
        {
            CardConfirmation.Instance.OnConfirmCardChoiceEvent -= HandleConfirmCardChoice;
            IsConfirmCardEventBinded = false;
        }
            
    }
    #endregion


}
