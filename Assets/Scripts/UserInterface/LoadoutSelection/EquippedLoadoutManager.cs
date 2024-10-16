using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.LoadoutSelection;
using UnityEngine;
using PlayerCore.Upgrades.Base;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.LoadoutSelection
{
    public class EquippedLoadoutManager : MonoBehaviour
    {
        [Header("Dependencies")]
        public LoadoutSelectionManager loadoutLayoutManager;
        public GameObject equippedLoadoutParent;
        
        public event Action OnLoadoutUpdated;
        public event Action<FLoadoutCardObj> OnLoadoutRemoved;
        
        private void OnEnable()
        {
            LoadoutCardUI[] loadoutCards = equippedLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

            foreach (LoadoutCardUI loadoutCard in loadoutCards)
            {
                loadoutCard.OnCardClicked += HandleCardRemoved;
            }
        }

        private void OnDisable()
        {
            if (equippedLoadoutParent != null)
            {
                LoadoutCardUI[] loadoutCards = equippedLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

                foreach (LoadoutCardUI loadoutCard in loadoutCards)
                {
                    if (loadoutCard != null)
                    {
                        loadoutCard.OnCardClicked -= HandleCardRemoved;
                    }
                }
            }
        }

        private void Start()
        {
            InitializeEquippedLoadout();
        }

        public bool AddUpgrade(UpgradeDefinitionSO selectedUpgrade)
        {
            //if the active objs are equal or exceed, means player has selected the max slots.
            if (GetActiveLoadoutCount() == LoadoutManager.Instance.cachedMaxSlots)
            {
                return false;
            }
            
            GameObject loadoutCardObj = GetFirstInactiveLoadout();
            if (!loadoutCardObj) return false;
            
            LoadoutCardUI loadoutCardUI = loadoutCardObj.GetComponent<LoadoutCardUI>();
            if (loadoutCardUI)
            {
                //setup card data
                loadoutCardUI.InitializeCard(new FLoadoutCardObj(selectedUpgrade));
                loadoutCardObj.SetActive(true);
                return true;
            }

            //default to false if no loadout card UI found.
            return false;
        }

        private void HandleCardRemoved(FLoadoutCardObj fLoadoutCardInfo)
        {
            OnLoadoutRemoved?.Invoke(fLoadoutCardInfo);
            OnLoadoutUpdated?.Invoke();
        }

        #region Internal methods
        private void InitializeEquippedLoadout()
        {
            var activeLoadoutObjList = GetAllEquippedLoadoutObj();

            foreach (GameObject loadoutObj in activeLoadoutObjList)
            {
                loadoutObj.SetActive(false);
            }

            OnLoadoutUpdated?.Invoke();
        }

        private GameObject GetFirstInactiveLoadout()
        {
            //return first inactive loadout.
            return GetAllEquippedLoadoutObj().FirstOrDefault(loadoutObj => !loadoutObj.activeSelf);
        }

        private int GetActiveLoadoutCount()
        {
            if(equippedLoadoutParent == null) return 0;
            
            //return number for child in parent that is active.
            int activeCount = (from Transform child in equippedLoadoutParent.transform where child.gameObject.activeSelf select child).Count();

            return activeCount;
        }
        
        private List<GameObject> GetAllEquippedLoadoutObj()
        {
            //return all active loadout objects.
            return (from Transform child in equippedLoadoutParent.transform select child.gameObject).ToList();
        }
        #endregion
    }
}
