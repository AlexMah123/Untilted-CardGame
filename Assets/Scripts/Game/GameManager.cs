using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Player Objects")]
    public GameObject humanPlayerObj;
    public GameObject aiPlayerObj;

    [Header("Player Data")]
    [HideInInspector] public Player humanPlayer;
    [HideInInspector] public AIPlayer computerPlayer;

    //declaration of events
    public event Action OnClearCardHandEvent;
    public event Action OnStartNewTurnEvent;
  
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

    public void HandleConfirmCardChoice(ChoiceCardUI cardUI)
    {
        humanPlayer.ChoiceComponent.currentChoice = cardUI.gameChoice;

        //trigger resolution of the round
        PlayRound();
    }

    public void PlayRound()
    {
        computerPlayer.ChoiceComponent.currentChoice = computerPlayer.GetChoice();

        //ai player update its Ai Module
        computerPlayer.aiModuleConfig.UpdateAIModule(humanPlayer.ChoiceComponent.currentChoice);

        //#DEBUG
        Debug.Log($"Human Player has selected {humanPlayer.ChoiceComponent.currentChoice}");
        Debug.Log($"AI Player has selected {computerPlayer.ChoiceComponent.currentChoice}");

        //get the result based on the played choice, then evaluate them (broadcast all the neccesary events)
        var roundResult = GameUtilsLibrary.GetGameResult(humanPlayer.ChoiceComponent.currentChoice, computerPlayer.ChoiceComponent.currentChoice);
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

    [ContextMenu("GameManager/Clear Round")]
    public void ClearRound()
    {
        OnClearCardHandEvent?.Invoke();
    }


    #region Internal Functions
    private void LoadPlayersData(GameData data)
    {
        if(LevelDataManager.Instance.currentSelectedLevelSO == null)
        {
            throw new NullReferenceException("LevelDataManager does not have a levelSO selected");
        }

        //load from LevelDataManager
        var humanPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.humanPlayer;
        var computerPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.aiPlayer;

        //cached
        humanPlayer = humanPlayerObj.GetComponent<Player>();
        computerPlayer = aiPlayerObj.GetComponent<AIPlayer>();

        if (humanPlayer == null || computerPlayer == null)
        {
            Debug.LogError(
                (humanPlayer == null ? "human player is null. " : "") +
                (computerPlayer == null ? "Ai player is null. " : "")
            );
        }

        //#TODO: technically load from save file
        humanPlayer.statsConfig = LevelDataManager.Instance.currentSelectedLevelSO.humanPlayer.StatsConfig;

        foreach(UpgradeType upgradeType in data.playerEquippedUpgrades)
        {
            humanPlayer.ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeType);
        }
        humanPlayer.LoadComponents();

        //=======================================================================================================
        computerPlayer.statsConfig = computerPlayerData.StatsConfig;
        computerPlayer.aiModuleConfig = computerPlayerData.AiModule;

        foreach(UpgradeDefinitionSO upgradeSO in computerPlayerData.upgradesEquipped)
        {
            computerPlayer.ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeSO);
        }

        computerPlayer.LoadComponents();
    }

    private void EvaluateResults(GameResult roundResult)
    {
        //#TODO: call the players win lose draw conditions
        switch(roundResult)
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
        if(humanPlayer.HealthComponent.healthAmount <= 0)
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

        LoadPlayersData(data);
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
