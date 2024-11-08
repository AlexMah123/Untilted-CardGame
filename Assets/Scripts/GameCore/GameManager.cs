using System;
using System.Linq;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using GameCore.SceneTransition;
using GameCore.TurnSystem;
using LevelCore.LevelManager;
using PlayerCore;
using PlayerCore.AIPlayer;
using PlayerCore.PlayerComponents;
using UnityEngine;
using UserInterface.Cards.ChoiceCard;
using UserInterface.Gameplay;

namespace GameCore
{
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
        public TurnSystemManager TurnSystemManager => TurnSystemManager.Instance;

        
        [Header("Player Data")]
        public Player player;
        public AIPlayer aiPlayer;

        [Header("GameManager Config")]
        [SerializeField] Transition rewardTransitionType;

        //declaration of events
        public event Action<GameResult> OnLevelCompleted;

        //Interface
        public event Action OnSaveDataLoaded;

        //flag
        private bool isConfirmCardEventBinded = false;
        private bool isAllDataLoadedEventBinded = false;

        private void OnEnable()
        {
            if (!isConfirmCardEventBinded)
            {
                BindConfirmCardChoiceEvent();
            }
            
            if (!isAllDataLoadedEventBinded)
            {
                BindAllDataLoadedEvent();
            }
        }
        private void OnDisable()
        {
            if (isConfirmCardEventBinded)
            {
                UnbindConfirmCardChoiceEvent();
            }
            
            if (isAllDataLoadedEventBinded)
            {
                UnbindAllDataLoadedEvent();
            }

            UnbindPlayersHealthZeroEvent();
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
            
            if (!isAllDataLoadedEventBinded)
            {
                BindAllDataLoadedEvent();
            }

            BindPlayersHealthZeroEvent();
        }
        
        private void HandleGameStart()
        {
            //Start game after everything has been loaded
            TurnSystemManager.Instance.HandleTurnHasCompleted();
        }

        public void HandleConfirmCardChoice(ChoiceCardUI cardUI)
        {
            if (player == null)
            {
                Debug.LogError("player reference not attached");
                return;
            }

            player.ChoiceComponent.currentChoice = cardUI.gameChoice;
            TurnSystemManager.ChangePhase(TurnSystemManager.EnemyPhase);
        }
        private void HandleOnHumanPlayerLose()
        {
            //#DEBUG
            Debug.Log("Player has lost");

            OnLevelCompleted?.Invoke(GameResult.Lose);
        }

        private void HandleOnAIPlayerLose()
        {
            //#DEBUG
            Debug.Log("Player has won");

            OnLevelCompleted?.Invoke(GameResult.Win);

            //save game
            SaveSystemManager.Instance.SaveGame();
        }

        #region Internal Functions

        public (GameChoice playerChoice, GameChoice aiChoice) GetFinalChoice()
        {
            //#DEBUG
            Debug.Log($"Human Player has selected {player.GetChoice()}");
            Debug.Log($"AI Player has selected {aiPlayer.GetChoice()}");
            
            return (player.GetChoice(), aiPlayer.GetChoice());
        }

        public bool EvaluateResults(ref GameResult initialResult)
        {
            //convert relative playerResult to the enemy. (if player win, aiPlayer loses, etc)
            GameResult aiPlayerInitialResult = GameUtilsLibrary.ConvertToEnemyResult(initialResult);

            //result is relative to player. cache these values.
            GameResult alteredPlayerResult = initialResult;
            GameResult alteredAIPlayerResult = aiPlayerInitialResult;
            
            //check if there are any altering effects for player based on the initial round result (players altering effect will not trigger this)
            alteredPlayerResult = player.ActiveLoadoutComponent.ApplyResultAlteringEffect(alteredPlayerResult);
            
            //check if there any altering effects for enemy based on the initial round result (players altering effect will not trigger this)
            alteredAIPlayerResult = aiPlayer.ActiveLoadoutComponent.ApplyResultAlteringEffect(alteredAIPlayerResult);
            
            //if either player or enemy has altered result, change the result.
            if(alteredPlayerResult != initialResult)
            {
                initialResult = alteredPlayerResult;
            }
            else if(alteredAIPlayerResult != aiPlayerInitialResult)
            {
                initialResult = GameUtilsLibrary.ConvertToEnemyResult(alteredAIPlayerResult);
            }
            
            //default return false, the results is the same
            return alteredPlayerResult != initialResult || alteredAIPlayerResult != aiPlayerInitialResult;
        }

        public void ApplyRoundEffects(GameResult cachedResult)
        {
            //result is relative to the player (player win,lose,draw)
            switch (cachedResult)
            {
                case GameResult.Win: // player win, aiplayer lose
                    player.ActiveLoadoutComponent.ApplyOnWinEffects();
                    aiPlayer.ActiveLoadoutComponent.ApplyOnLoseEffect();
                    break;
                    
                case GameResult.Lose: // player lose, aiplayer win
                    player.ActiveLoadoutComponent.ApplyOnLoseEffect();
                    aiPlayer.ActiveLoadoutComponent.ApplyOnWinEffects();
                    break;
                    
                case GameResult.Draw: // both player draw
                    player.ActiveLoadoutComponent.ApplyOnDrawEffect();
                    aiPlayer.ActiveLoadoutComponent.ApplyOnDrawEffect();
                    break;
            }
        }

        public void ApplyDamage(GameResult roundResult)
        {
            switch (roundResult)
            {
                case GameResult.Win:
                    //human player deal dmg to opposing player
                    player.DamageComponent.DealDamage(aiPlayer, player.DamageComponent.attack);
                    break;

                case GameResult.Lose:
                    //aiPlayer player deal dmg to human player
                    aiPlayer.DamageComponent.DealDamage(player, aiPlayer.DamageComponent.attack);
                    break;

                case GameResult.Draw:
                    //do nothing
                    break;
            }

            //#DEBUG
            Debug.Log($"Human Player has {roundResult}");
        }
        
        #endregion

        #region SavableData System
        public void LoadData(GameData data)
        {
            //#DEBUG
            Debug.Log($"Currently loaded: {LevelDataManager.Instance.currentSelectedLevelSO.levelName}");

            OnSaveDataLoaded?.Invoke();
        }

        public void SaveData(ref GameData data)
        {
            UpdateLevelCompletionStatus(ref data);
        }
        
        private void UpdateLevelCompletionStatus(ref GameData data)
        {
            if (LevelDataManager.Instance.currentSelectedLevelSO == null) 
            {
                Debug.LogWarning("LevelManager does not exist, currentSelectedLevelSO is null");
                return; 
            }

            var currentLevel = LevelDataManager.Instance.currentSelectedLevelSO;

            var completedLevelData = data.levelCompletionData.FirstOrDefault(level => level.levelName == currentLevel.levelName);

            if(completedLevelData != null)
            {
                completedLevelData.isCompleted = true;
            }
            else
            {
                data.levelCompletionData.Add(new LevelCompletionData
                {
                    levelName = currentLevel.levelName,
                    isCompleted = true
                });
            }

        }
        #endregion
        
        #region BindOnAllDataLoaded From SaveSystem
        private void BindAllDataLoadedEvent()
        {
            if (SaveSystemManager.Instance != null)
            {
                SaveSystemManager.Instance.OnAllSaveDataLoaded += HandleGameStart;
                isAllDataLoadedEventBinded = true;
            }
        }
        private void UnbindAllDataLoadedEvent()
        {
            if (SaveSystemManager.Instance != null)
            {
                SaveSystemManager.Instance.OnAllSaveDataLoaded -= HandleGameStart;
                isAllDataLoadedEventBinded = false;
            }
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

        #region Bind PlayerHealthZero Event
        private void BindPlayersHealthZeroEvent()
        {
            if(player)
            {
                player.HealthComponent.OnHealthZero += HandleOnHumanPlayerLose;
            }

            if(aiPlayer)
            {
                aiPlayer.HealthComponent.OnHealthZero += HandleOnAIPlayerLose;
            }
        }

        private void UnbindPlayersHealthZeroEvent()
        {
            if (player)
            {
                player.HealthComponent.OnHealthZero -= HandleOnHumanPlayerLose;
            }

            if (aiPlayer)
            {
                aiPlayer.HealthComponent.OnHealthZero -= HandleOnAIPlayerLose;
            }
        }

        #endregion

        #region Helper
#if UNITY_EDITOR
        private void ClearEditorLog()
        {
            EditorUtilsLibrary.ClearLogConsole();
        }
#endif
        #endregion
    }
}