using GameCore.TurnSystem.Phases.Base;
using PlayerCore;
using PlayerCore.AIPlayer;
using PlayerCore.PlayerComponents;
using UnityEngine;

namespace GameCore.TurnSystem.Phases
{
    public class EnemyPhase : Phase
    {
        GameManager GameManager => GameManager.Instance;

        protected override void OnStart(Player player, AIPlayer aiPlayer)
        {
            //#TODO: Play animations etc etc. do enemy decision

            //ai player update its Ai Module
            if (aiPlayer != null)
            {
                ChoiceComponent aiPlayerChoiceComponent = aiPlayer.ChoiceComponent;

                aiPlayerChoiceComponent.currentChoice = aiPlayer.GetChoice();
                aiPlayer.aiModuleConfig.UpdateAIModule(player.GetChoice());
            }

            Controller.ChangePhase(Controller.EvaluationPhase);
        }

        protected override void OnUpdate(Player player, AIPlayer aiPlayer)
        {
        }

        protected override void OnEnd(Player player, AIPlayer aiPlayer)
        {
        }
    }
}