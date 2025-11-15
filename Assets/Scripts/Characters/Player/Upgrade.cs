using System;

[Serializable]
public class Upgrade {

    //Upgrade
    private readonly string key;

    public string Key => key;

    //Level
    public int Level { get; private set; }
    public int LevelMax { get; private set; }

    public bool CanUpgrade => Level < LevelMax;

    public const int DEFAULT_LEVEL_MIN = 1;
    public const int DEFAULT_LEVEL_MAX = 5;

    //Cost
    public int CostBase { get; private set; }
    public int CostPerLevel { get; private set; }

    public int Cost => CostBase + (Level - 1) * CostPerLevel;

    public const int DEFAULT_COST_BASE = 30;
    public const int DEFAULT_COST_PER_LEVEL = 15;


    //Constructor
    public Upgrade(
        //Upgrade
        string key,
        //Level
        int level = DEFAULT_LEVEL_MIN,
        int levelMax = DEFAULT_LEVEL_MAX,
        //Cost
        int costBase = DEFAULT_COST_BASE,
        int costPerLevel = DEFAULT_COST_PER_LEVEL
    ) {
        //Upgrade
        this.key = key;

        //Level
        Level = level;
        LevelMax = levelMax;

        //Cost
        CostBase = costBase;
        CostPerLevel = costPerLevel;
    }

    //Upgrade
    public bool TryUpgrade(Loadout loadout) {
        //Check if can upgrade
        if (!CanUpgrade) return false;

        //Pay for upgrade
        if (!loadout.SpendGold(Cost)) return false;

        //Upgrade
        SetLevel(Level + 1);
        loadout.SetUpgrade(Key, Level);

        //Success
        return true;
    }

    //Level
    public void SetLevel(int level) {
        Level = level;
    }

    public void SetLevelMax(int levelMax) {
        LevelMax = levelMax;
    }

    //Cost
    public void SetCost(int costBase, int costPerLevel) {
        CostBase = costBase;
        CostPerLevel = costPerLevel;
    }

}
