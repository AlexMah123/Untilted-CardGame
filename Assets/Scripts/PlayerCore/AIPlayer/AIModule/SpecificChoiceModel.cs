using PlayerComponents.ChoiceComponent;
using PlayerCore.AIPlayer.AIModule.Base;
using UnityEngine;

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
