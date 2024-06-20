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
    public event Action OnStartNewTurnEvent;
  
    private bool isConfirmCardEventBinded = false;
    
    private void OnEnable()
    {
        if (CardConfirmation.Instance != null)
        {
            BindConfirmCardChoiceEvent();
        }
    }
    private void OnDisable()
    {
        if(isConfirmCardEventBinded)
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
        if (!isConfirmCardEventBinded)
        {
            BindConfirmCardChoiceEvent();
        }
    }

    public void CreatePlayers()
    {
        //#TODO: Have a function to reload the levels and create players?.
    }

    public void HandleConfirmCardChoice(CardUI cardUI)
    {
        humanPlayer.currentChoice = cardUI.gameChoice;

        //trigger resolution of the round
        PlayRound();
    }

    public void PlayRound()
    {
        aiPlayer.currentChoice = aiPlayer.GetChoice();

        //if player is an ai player, update its Ai Module
        if(aiPlayer.GetComponent<AIPlayer>())
        {
            aiPlayer.GetComponent<AIPlayer>().aiModuleConfig.UpdateAIModule(humanPlayer.currentChoice);
        }

        Debug.Log($"Human Player has selected {humanPlayer.currentChoice}");
        Debug.Log($"AI Player has selected {aiPlayer.currentChoice}");

        //#TODO: add some sort of delay between rounds for animation?
        GetGameResult(humanPlayer.currentChoice, aiPlayer.currentChoice);

        //After results, reset the turns
        //broadcast event, primarily binded to PlayerHandUIManager
        OnClearCardHandEvent?.Invoke();

        //broadcast event, primarily binded to TurnSystemManager
        OnStartNewTurnEvent?.Invoke();

        //TESTING
        Invoke(nameof(ClearEditorLog), 2f);
    }

    #region Internal Functions
    private GameResult GetGameResult(GameChoice humanPlayerChoice, GameChoice aiPlayerChoice)
    {
        var result = GameResult.NONE;

        if (humanPlayerChoice == aiPlayerChoice)
        {
            result = GameResult.DRAW;
        }
        else if (humanPlayerChoice == GameChoice.ROCK && aiPlayerChoice == GameChoice.SCISSOR ||
                humanPlayerChoice == GameChoice.PAPER && aiPlayerChoice == GameChoice.ROCK ||
                humanPlayerChoice == GameChoice.SCISSOR && aiPlayerChoice == GameChoice.PAPER)
        {
            result = GameResult.WIN;
        }
        else
        {
            result = GameResult.LOSE;
        }

        Debug.Log($"Human Player has {result}");

        return result;
    }

    private void ClearEditorLog()
    {
        UtilsLibrary.ClearLogConsole();
    }
    #endregion

    #region Bind CardConfirmation event
    private void BindConfirmCardChoiceEvent()
    {
        if (CardConfirmation.Instance != null)
        {
            CardConfirmation.Instance.OnConfirmCardChoiceEvent += HandleConfirmCardChoice;
            isConfirmCardEventBinded = true;
        }
    }

    private void UnbindConfirmCardChoiceEvent()
    {
        if (CardConfirmation.Instance != null)
        {
            CardConfirmation.Instance.OnConfirmCardChoiceEvent -= HandleConfirmCardChoice;
            isConfirmCardEventBinded = false;
        }
            
    }
    #endregion


}
