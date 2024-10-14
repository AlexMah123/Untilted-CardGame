using UnityEngine;

using PlayerComponents;
using PlayerCore.AIPlayer.AIModule.Base;

namespace PlayerCore.AIPlayer.AIModule
{
    [CreateAssetMenu(menuName = "Enemy/Decision/SpecificChoiceModel")]
    public class SpecificChoiceModel : AIDecision
    {
        [SerializeField] GameChoice choice;

        public override GameChoice MakeDecision()
        {
            return choice;
        }

        public override void ResetAIConfig()
        {
        
        }
    }
}
