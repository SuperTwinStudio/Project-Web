using System;

[Serializable]
public class Upgrade {

    //Upgrade
    private readonly string key;

    public string Key => key;

    //Level
    private int level;
    private int levelMax;

    public int Level => level;
    public int LevelMax => levelMax;
    public bool CanUpgrade => Level < LevelMax;

    public const int DEFAULT_LEVEL_MIN = 1;
    public const int DEFAULT_LEVEL_MAX = 5;

    //Cost
    private int costBase;
    private int costPerLevel;

    public int Cost => costBase + (level - 1) * costPerLevel;

    public const int DEFAULT_COST_BASE = 10;
    public const int DEFAULT_COST_PER_LEVEL = 10;


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
        this.level = level;
        this.levelMax = levelMax;

        //Cost
        this.costBase = costBase;
        this.costPerLevel = costPerLevel;
    }

    //Upgrade
    public bool TryUpgrade(Loadout loadout) {
        //Check if can upgrade
        if (!CanUpgrade) return false;

        //Pay for upgrade
        if (!loadout.Expend(Cost)) return false;

        //Upgrade
        SetLevel(Level + 1);
        loadout.SetUpgrade(Key, Level);

        //Success
        return true;
    }

    //Level
    public void SetLevel(int level) {
        this.level = level;
    }

    public void SetLevelMax(int levelMax) {
        this.levelMax = levelMax;
    }

    //Cost
    public void SetCost(int costBase, int costPerLevel) {
        this.costBase = costBase;
        this.costPerLevel = costPerLevel;
    }

}
