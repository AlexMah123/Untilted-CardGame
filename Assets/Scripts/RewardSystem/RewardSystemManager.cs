using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardSystemManager : MonoBehaviour
{
    [Header("RewardUI Config")]
    [SerializeField] LoadoutCardUI rewardPrefab;

    [Header("Reward Display Config")]
    [SerializeField] RectTransform rewardContainer;
    [SerializeField] RectTransform content;
    [SerializeField] GridLayoutGroup gridLayout;

    [Header("Runtime Data")]
    [SerializeField] List<UpgradeDefinitionSO> upgradeRewardList;
    List<LoadoutCardUI> rewardUIList;

    void Start()
    {
        if (LevelDataManager.Instance.currentSelectedLevelSO == null)
        {
            Debug.LogWarning("LevelManager does not exist, currentSelectedLevelSO is null");
            return;
        }

        //Based on the level, there is a set rewardList
        upgradeRewardList = LevelDataManager.Instance.currentSelectedLevelSO.rewardList;

        InitializeRewardContainer();
        UpdateRewardDisplay();
    }

    private void InitializeRewardContainer()
    {
        //initialize the preset amount of rewardUI
        rewardUIList = content.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();
    }

    private void UpdateRewardDisplay()
    {
        //reset all rewardUI
        foreach (LoadoutCardUI reward in rewardUIList)
        {
            reward.gameObject.SetActive(false);
        }

        //based on rewardList, display that many reward UI
        for (int i = 0; i < upgradeRewardList.Count; i++)
        {
            LoadoutCardUI rewardUI = GetReward();

            if(rewardUI)
            {
                //populate the data 
                rewardUI.InitializeCard(new LoadoutCardGOInfo(upgradeRewardList[i]));
                rewardUI.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("failed to create rewardUI");
                break;
            }
        }

        AdjustRewardContainerSize(upgradeRewardList.Count);
    }

    private LoadoutCardUI GetReward()
    {
        LoadoutCardUI availableRewardUI = rewardUIList.FirstOrDefault(x => x.gameObject.activeInHierarchy == false);
        bool failedToGetExistingReward = availableRewardUI == null;

        if(failedToGetExistingReward)
        {
            availableRewardUI = Instantiate(rewardPrefab, content);
            rewardUIList.Add(availableRewardUI);
        }

        return availableRewardUI;
    }

    private void AdjustRewardContainerSize(int rewardCount)
    {
        if (rewardCount == 0)
        {
            // If no rewards, hide the reward container
            rewardContainer.sizeDelta = new Vector2(0, 0);
        }
        else
        {
            //force update the layout
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

            Vector2 preferredSize = content.GetComponent<RectTransform>().sizeDelta;

            float totalWidth = preferredSize.x / 2f;
            float totalHeight = preferredSize.y / 1.5f;

            rewardContainer.sizeDelta = new Vector2(totalWidth, totalHeight);
        }
    }
}
