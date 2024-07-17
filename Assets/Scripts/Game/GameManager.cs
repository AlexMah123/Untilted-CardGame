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
  
    //flag
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

    public void HandleConfirmCardChoice(ChoiceCardUI cardUI)
    {
        humanPlayer.ChoiceComponent.currentChoice = cardUI.gameChoice;

        //trigger resolution of the round
        PlayRound();
    }

    public void PlayRound()
    {
        aiPlayer.ChoiceComponent.currentChoice = aiPlayer.GetChoice();

        //if player is an ai player, update its Ai Module
        if(aiPlayer.GetComponent<AIPlayer>())
        {
            aiPlayer.GetComponent<AIPlayer>().aiModuleConfig.UpdateAIModule(humanPlayer.ChoiceComponent.currentChoice);
        }

        //#DEBUG
        Debug.Log($"Human Player has selected {humanPlayer.ChoiceComponent.currentChoice}");
        Debug.Log($"AI Player has selected {aiPlayer.ChoiceComponent.currentChoice}");

        //get the result based on the played choice, then evaluate them (broadcast all the neccesary events)
        var roundResult = GameUtilsLibrary.GetGameResult(humanPlayer.ChoiceComponent.currentChoice, aiPlayer.ChoiceComponent.currentChoice);
        EvaluateResults(roundResult);

        //After results, reset the turns
        //broadcast event, primarily binded to PlayerHandUIManager
        OnClearCardHandEvent?.Invoke();

        //#TODO: add some sort of delay between rounds for animation?
        //broadcast event, primarily binded to TurnSystemManager
        OnStartNewTurnEvent?.Invoke();

#if UNITY_EDITOR
        //TESTING
        Invoke(nameof(ClearEditorLog), 3f);
#endif
        
    }

    #region Internal Functions

    private void EvaluateResults(GameResult roundResult)
    {
        //#TODO: call the players win lose draw conditions
        switch(roundResult)
        {
            case GameResult.WIN:
                //human player deal dmg to opposing player
                humanPlayer.DamageComponent.DealDamage(aiPlayer, humanPlayer.DamageComponent.damageAmount);
                break;

            case GameResult.LOSE:
                //aiPlayer player deal dmg to human player
                aiPlayer.DamageComponent.DealDamage(humanPlayer, aiPlayer.DamageComponent.damageAmount);
                break;

            case GameResult.DRAW:
                //do nothing
                break;
        }

        //#TODO: Check players healths

        //#DEBUG
        Debug.Log($"Human Player has {roundResult}");
    }

#if UNITY_EDITOR
    private void ClearEditorLog()
    {
        EditorUtilsLibrary.ClearLogConsole();
    }
#endif

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
