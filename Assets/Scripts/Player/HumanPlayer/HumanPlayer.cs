public class HumanPlayer : Player
{
    protected override void LoadPlayerData(GameData data)
    {
        //load from LevelDataManager
        var humanPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.humanPlayer;

        baseStatsConfig = humanPlayerData.baseStatsConfig;
        
        //load stats upgrade from save file
        upgradeStats = new PlayerStats
        {
            health = data.upgradedPlayerStats.health,
            damage = data.upgradedPlayerStats.damage,
            cardSlots = data.upgradedPlayerStats.cardSlots,
            energy = data.upgradedPlayerStats.energy
        };

        //load cardUpgrades from save file
        foreach (UpgradeType upgradeType in data.playerEquippedUpgrades)
        {
            ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeType);
        }
    }
}
