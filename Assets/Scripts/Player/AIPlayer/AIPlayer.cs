using UnityEngine;

public class AIPlayer : Player
{
    public AIDecision aiModuleConfig;

    protected override void Awake()
    {
        base.Awake();

        aiModuleConfig.InitializeAIConfig(ChoiceComponent);
        //ChoiceComponent.SealChoice(GameChoice.ROCK);
        //ChoiceComponent.SealChoice(GameChoice.PAPER);
    }

    public override void LoadComponents()
    {
        base.LoadComponents();

        aiModuleConfig.InitializeAIConfig(ChoiceComponent);
    }

    public override GameChoice GetChoice()
    {
        if (aiModuleConfig == null)
        {
            throw new MissingComponentException("AI Module is not assigned");
        }

        return aiModuleConfig.MakeDecision();
    }

    protected override void LoadPlayerData(GameData data)
    {
        var computerPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.aiPlayer;

        statsConfig = computerPlayerData.StatsConfig;
        aiModuleConfig = computerPlayerData.AiModule;

        foreach (UpgradeDefinitionSO upgradeSO in computerPlayerData.upgradesEquipped)
        {
            ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeSO);
        }

        LoadComponents();
    }

}
