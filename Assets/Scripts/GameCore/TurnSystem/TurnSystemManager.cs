using System;
using GameCore.SaveSystem;
using GameCore.TurnSystem.Phases;
using GameCore.TurnSystem.Phases.Base;
using PlayerCore;
using PlayerCore.AIPlayer;
using UnityEngine;

namespace GameCore.TurnSystem
{
    public class TurnSystemManager : MonoBehaviour
    {
        public static TurnSystemManager Instance;

        //#TODO: currently not in use, used when enemy has their own turn
        private Player Player => GameManager.Instance.player;
        private AIPlayer AIPlayer => GameManager.Instance.AIPlayer;

        [Header("Runtime values")]
        public Phase CurrentPhase;
        public int turnCount;

        //declaration of possible turns
        public readonly StartOfRound StartOfRound = new();
        public readonly PlayerPhase PlayerPhase = new();
        public readonly EnemyPhase EnemyPhase = new();
        public readonly EvaluationPhase EvaluationPhase = new();
        

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

        void Update()
        {
            if (CurrentPhase != null)
            {
                CurrentPhase.OnUpdatePhase(Player, AIPlayer);
            }
        }

        [ContextMenu("TurnSystemManager/StartRound")]
        public void HandleTurnHasCompleted()
        {
            ChangePhase(StartOfRound);
        }

        public void ChangePhase(Phase newPhase)
        {
            if (CurrentPhase != null)
            {
                CurrentPhase.OnEndPhase(Player, AIPlayer);
            }
            
            CurrentPhase = newPhase;
            CurrentPhase.OnStartPhase(this, Player, AIPlayer);
        }
    }
}
