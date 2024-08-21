using System;
using UnityEngine;

public enum GameResult
{
    None,
    Win,
    Lose,
    Draw
}

public class GameManager : MonoBehaviour, ISavableData
{
    public static GameManager Instance;

    [Header("Player Data")]
    public Player humanPlayer;
    public AIPlayer computerPlayer;

    //declaration of events
    public event Action OnClearCardInHand;
    public event Action OnTurnCompleted;

    //Interface
    public event Action OnSaveDataLoaded;

    //flag
    private bool isConfirmCardEventBinded = false;

    private void OnEnable()
    {
        if (!isConfirmCardEventBinded)
        {
            BindConfirmCardChoiceEvent();
        }
    }
    private void OnDisable()
    {
        if (isConfirmCardEventBinded)
        {
            UnbindConfirmCardChoiceEvent();
        }
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
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

    public void HandleConfirmCardChoice(ChoiceCardUI cardUI)
    {
        if (humanPlayer == null)
        {
            Debug.LogError("player reference not attached");
            return;
        }

        humanPlayer.ChoiceComponent.currentChoice = cardUI.gameChoice;

        //trigger resolution of the round
        PlayRound();
    }

    public void PlayRound()
    {
        if (computerPlayer == null)
        {
            Debug.LogError("ai player reference not attached");
            return;
        }

        ChoiceComponent humanPlayerChoice = humanPlayer.ChoiceComponent;
        ChoiceComponent computerPlayerChoice = computerPlayer.ChoiceComponent;

        //ai player update its Ai Module
        computerPlayerChoice.currentChoice = computerPlayer.GetChoice();
        computerPlayer.aiModuleConfig.UpdateAIModule(humanPlayerChoice.currentChoice);

        //#DEBUG
        Debug.Log($"Human Player has selected {humanPlayerChoice.currentChoice}");
        Debug.Log($"AI Player has selected {computerPlayerChoice.currentChoice}");

        //get the result based on the played choice, then evaluate them (broadcast all the neccesary events)
        var roundResult = GameUtilsLibrary.GetGameResult(humanPlayerChoice.currentChoice, computerPlayerChoice.currentChoice);
        EvaluateResults(roundResult);

        //After results, reset the turns
        //broadcast event, primarily binded to PlayerHandUIManager
        OnClearCardInHand?.Invoke();

        //#TODO: add some sort of delay between rounds for animation?
        //broadcast event, primarily binded to TurnSystemManager
        OnTurnCompleted?.Invoke();

#if UNITY_EDITOR
        //TESTING
        Invoke(nameof(ClearEditorLog), 3f);
#endif

    }

    [ContextMenu("GameManager/Clear Round")]
    public void ClearRound()
    {
        OnClearCardInHand?.Invoke();
    }


    #region Internal Functions
    private void EvaluateResults(GameResult roundResult)
    {
        //#TODO: call the players win lose draw conditions
        switch (roundResult)
        {
            case GameResult.Win:
                //human player deal dmg to opposing player
                humanPlayer.DamageComponent.DealDamage(computerPlayer, humanPlayer.DamageComponent.damageAmount);
                break;

            case GameResult.Lose:
                //aiPlayer player deal dmg to human player
                computerPlayer.DamageComponent.DealDamage(humanPlayer, computerPlayer.DamageComponent.damageAmount);
                break;

            case GameResult.Draw:
                //do nothing
                break;
        }

        //#TODO: Check players healths
        if (humanPlayer.HealthComponent.healthAmount <= 0)
        {
            //#DEBUG
            Debug.Log("Player has lost");
        }

        if (computerPlayer.HealthComponent.healthAmount <= 0)
        {
            //#DEBUG
            Debug.Log("Player has won");
        }

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

    #region SavableData Interface
    public void LoadData(GameData data)
    {
        //#DEBUG
        Debug.Log($"Currently loaded: {LevelDataManager.Instance.currentSelectedLevelSO.levelName}");

        OnSaveDataLoaded?.Invoke();
    }

    public void SaveData(ref GameData data)
    {
        //mark when level is complete
    }

    #endregion

    #region Bind CardConfirmation event
    private void BindConfirmCardChoiceEvent()
    {
        if (CardConfirmation.Instance != null)
        {
            CardConfirmation.Instance.OnConfirmCardChoice += HandleConfirmCardChoice;
            isConfirmCardEventBinded = true;
        }
    }

    private void UnbindConfirmCardChoiceEvent()
    {
        if (CardConfirmation.Instance != null)
        {
            CardConfirmation.Instance.OnConfirmCardChoice -= HandleConfirmCardChoice;
            isConfirmCardEventBinded = false;
        }

    }
    #endregion


}
