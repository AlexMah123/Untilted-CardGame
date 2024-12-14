using PlayerCore.PlayerComponents;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.AbilityInput
{
    public class ChoiceSelectionPanel : MonoBehaviour
    {
        [Header("ChoiceSelection Config")]
        public Button confirmButton;
        
        [HideInInspector]
        public GameChoice selectedGameChoice;
        
        private void SetCachedGameChoice(GameChoice gameChoice)
        {
            selectedGameChoice = gameChoice;
        }
    }
}