using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.LoadoutSelection
{
    public class LoadoutSelectionPageManager : MonoBehaviour
    {
        public static LoadoutSelectionPageManager Instance;

        [SerializeField] private EquippedLoadoutManager equippedLoadoutManager;

        [SerializeField] Button previousButton;
        [SerializeField] Button nextButton;

        private int currentPageIndex = 0;
        private int totalUpgrades;

        public event Action<int> OnLoadoutPageUpdated;

        private void OnEnable()
        {
            if (equippedLoadoutManager != null)
            {
                equippedLoadoutManager.OnLoadoutUpdated += UpdateButtonState;
            }        
        }

        private void OnDisable()
        {
            if (equippedLoadoutManager != null)
            {
                equippedLoadoutManager.OnLoadoutUpdated -= UpdateButtonState;
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
            Invoke(nameof(UpdateButtonState), 0.1f);
        }

        public void PreviousPage()
        {
            ChangePage(-1);
        }

        public void NextPage()
        {
            ChangePage(1);
        }

        private void ChangePage(int direction)
        {
            currentPageIndex += direction;

            UpdateButtonState();

            //primarily binded to LoadoutSelectionManager
            OnLoadoutPageUpdated?.Invoke(currentPageIndex);
        }

        public void UpdateButtonState()
        {
            var loadoutManager = LoadoutSelectionManager.Instance;

            //null check since this is called multiple times in different timing, could be before things are initialized.
            if (loadoutManager == null || loadoutManager.cachedLoadoutData.totalUpgradesInGame == null)
            {
                return;
            }
            
            previousButton.interactable = currentPageIndex > 0;
            nextButton.interactable = (currentPageIndex + 1) * LoadoutSelectionManager.Instance.displayAmountPerPage < LoadoutSelectionManager.Instance.cachedLoadoutData.totalUpgradesInGame.Count;
        }
    }
}
