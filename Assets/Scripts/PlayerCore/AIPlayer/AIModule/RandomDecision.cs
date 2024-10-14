using UnityEngine;

using PlayerComponents;
using PlayerCore.AIPlayer.AIModule.Base;

namespace PlayerCore.AIPlayer.AIModule
{
    [CreateAssetMenu(menuName = "Enemy/Decision/RandomDecisionSO")]
    public class RandomDecision : AIDecision
    {
        public override GameChoice MakeDecision()
        {
            GameChoice aiDecision = RandomChoice();
            if (choiceComponent.IsChoiceAvailable(aiDecision))
            {
                return aiDecision;
            }
            else
            {
                return MakeDecision();
            }
        }

        public override void ResetAIConfig()
        {
            //empty
        }
    }
}
