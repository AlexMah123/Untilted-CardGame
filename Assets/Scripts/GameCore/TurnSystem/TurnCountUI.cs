using System;
using TMPro;
using UnityEngine;

namespace GameCore.TurnSystem
{
    public class TurnCountUI : MonoBehaviour
    {
        private TurnSystemManager turnSystemManager;

        [SerializeField] TextMeshProUGUI turnCountText;
        
        private void Start()
        {
            turnSystemManager = TurnSystemManager.Instance;

            if (turnSystemManager != null)
            {
                turnSystemManager.StartOfRound.TurnCountChanged += HandleTurnCountChanged;
            }
        }

        private void OnDisable()
        {
            if (turnSystemManager != null)
            {
                turnSystemManager.StartOfRound.TurnCountChanged -= HandleTurnCountChanged;
            }
        }
        
        private void HandleTurnCountChanged(int currentCount)
        {
            turnCountText.text = $"Turn: {currentCount}";
        }
        
    }
}