using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Resource
{
    public enum MissionState : int// TypeDefIndex: 33630
    {
        None = 0,
        Available = 1,
        Processing = 2,
        Completed = 3,
        Failed = 4
    }
    public enum TalentNodeType // TypeDefIndex: 33688
    {
        None = 0,
        CharBreak = 1,
        EquipBreak = 2,
        Attr = 3,
        PassiveSkill = 4,
        FactorySkill = 5
    }
    public enum GuideState // TypeDefIndex: 33634
    {
        None = 0,
        Processing = 1,
        KeyStepFinish = 2,
        Completed = 3
    }
    public enum AdventureTaskState // TypeDefIndex: 33702
    {
        None = 0,
        Processing = 1,
        Completed = 2,
        Rewarded = 3
    }
    public enum EntryState // TypeDefIndex: 24402
    {
        Empty = 0,
        DifferentMap = 1,
        SameMap = 2,
        LevelLoaded = 3,
        Invalid = 4
    }
    public enum AdventureTaskType // TypeDefIndex: 33687
    {
        None = 0,
        Daily = 1,
        AdventureBook = 2
    }
    public enum ItemStorageSpace // TypeDefIndex: 33575
    {
        None = 0,
        Isolate = 1,
        ValuableDepot = 2,
        BagAndFactoryDepot = 3
    }
    public enum ItemValuableDepotType : int // TypeDefIndex: 33573
    {
        Invalid = 0, //
        Weapon = 1,//
        WeaponGem = 2,//
        Equip = 3,//
        SpecialItem = 4,//
        MissionItem = 5,//
        Factory = 6,//
        HiddenItem = 7,//
        TDItem = 8,//
        TrialCharItem = 9,
        CommercialItem = 10
    }
    public enum ActorType // TypeDefIndex: 17934
    {
        None = 0,
        Character = 1,
        Interactive = 2,
        TmpObject = 3,
        Effect = 4
    }
    public enum ObjectType // TypeDefIndex: 24769
    {
        Invalid = 1,
        Character = 8,
        Enemy = 16,
        Interactive = 32,
        Projectile = 64,
        FactoryRegion = 128,
        Npc = 256,
        AbilityEntity = 512,
        CinematicEntity = 1024,
        RemoteFactoryEntity = 2048,
        Creature = 4096
    }

    public enum ObjectTypeIndex // TypeDefIndex: 24768
    {
        Invalid = 0,
        Character = 3,
        Enemy = 4,
        Interactive = 5,
        Projectile = 6,
        FactoryRegion = 7,
        Npc = 8,
        AbilityEntity = 9,
        CinematicEntity = 10,
        RemoteFactoryEntity = 11,
        Creature = 12,
        Enum = 13
    }
    public enum ForbidType // TypeDefIndex: 18780
    {
        ForbidSetSquad = 0,
        ForbidFactoryMode = 1,
        ForbidUseItem = 2,
        NoLoot = 3,
        ForbidCharSkill = 4,
        ForbidCharLvUp = 5,
        ForbidCharStageBreak = 6,
        ForbidChangeWeapon = 7,
        ForbidWeaponLvUp = 8,
        ForbidWeaponLvBreakthrough = 9,
        ForbidChangeGem = 10,
        ForbidChangeEquip = 11,
        ForbidChangeEquipMedicine = 12,
        HideSquadIcon = 13,
        ForbidAttack = 14,
        ForbidSprint = 15,
        ForbidWeaponRefineUpgrade = 16,
        OnlyClientEndmin = 17,
        HideMissionHud = 18,
        ForbidUseItemType = 19,
        NoRepatriateDamage = 20,
        DisableSwitchMode = 21,
        ShowEmptySwitchModeBtn = 22,
        HideGeneralAbility = 23,
        ForbidBattleEnterExitBark = 24,
        EnumMax = 25
    }
    public enum ScopeName // TypeDefIndex: 33636
    {
        None = 0,
        Main = 1,
        Blackbox = 2,
        RpgDungeon = 4,
        Training = 8,
        All = 15
    }

    public enum ScopeNameIndex // TypeDefIndex: 33635
    {
        Main = 0,
        Blackbox = 1,
        RpgDungeon = 2,
        Training = 3
    }
    public enum StatType // TypeDefIndex: 33663
    {
        None = 0,
        StatTest = 1,
        GemAttachNum = 2,
        UnlockTechNum = 3,
        TierLevelEquipProdeceNum = 4,
        MinerBuildNum = 5,
        ChestOpenNum = 6,
        DungeonSeriesCompleteNum = 7,
        EuqipSuitEffectNum = 8,
        ItemGainNum = 9,
        RacingPassNum = 10,
        GameCategoryPassNum = 11,
        EquipPutonNum = 12,
        CampRestNum = 13,
        ItemUseNum = 14,
        ChangeSceneGrade = 15,
        EquipMedicine = 16,
        DoodadTotalGot = 17,
        DoodadIdGot = 18,
        obtainCharNum = 19,
        GemForgeNum = 20,
        StatEnd = 499,
        DailyStatBegin = 500,
        DailyLogin = 501,
        DailyMissionComplete = 502,
        DailyStaminaCost = 503,
        DailyCharLevelUp = 504,
        DailyWeapeonLevelUp = 505,
        DailyCharSkillLevelUp = 507,
        DailyDungeonSeriesCompleteNum = 508,
        DailyEquipProduce = 509,
        DailyTraderOrderComplete = 510,
        DailySpaceshipShopBuy = 511,
        DailyDoodadTotalGot = 512,
        DailyManuallyWork = 513,
        DailyRacingPassNum = 514,
        DailyPresentGift = 515,
        DailyMonsterKillNum = 516,
        DailyGameCategoryPassNum = 517,
        DailyDoodadIdGot = 518,
        DailyStatEnd = 1000,
        WeeklyStatBegin = 1001,
        WeeklyStatEnd = 1500
    }
    public enum BitsetType // TypeDefIndex: 33649
    {
        None = 0,
        FoundItem = 1,
        Wiki = 2,
        UnreadWiki = 3,
        MonsterDrop = 4,
        GotItem = 5,
        AreaFirstView = 6,
        UnreadGotItem = 7,
        PRTS = 8,
        UnreadPRTS = 9,
        PRTSFirstLv = 10,
        PRTSTerminalContent = 11,
        LevelHaveBeen = 12,
        LevelMapFirstView = 13,
        UnreadFormula = 14,
        NewChar = 15,
        ElogChannel = 16,
        FMVWatched = 17,
        TimeLineWatched = 18,
        MapFilter = 19,
        FriendHasRequest = 20,
        EquipTechFormula = 21,
        RadioTrigger = 22,
        RemoteCommunicationFinish = 23,
        ChapterFirstView = 25,
        AdventureLevelRewardDone = 26,
        DungeonEntranceTouched = 27,
        EquipTechTier = 28,
        CharDoc = 30,
        CharVoice = 31,
        ReadingPop = 32,
        RewardIdDone = 33,
        PrtsInvestigate = 34,
        RacingReceivedBPNode = 35,
        RacingCompleteAchievement = 36,
        RacingReceivedAchievement = 37,
        NewSceneGradeUnlocked = 38,
        InteractiveActive = 39,
        MinePointFirstTimeCollect = 40,
        UnreadCharDoc = 41,
        UnreadCharVoice = 42,
        ItemSubmitRecycle = 43,
        AreaToastOnce = 44,
        UnreadEquipTechFormula = 45,
        PrtsInvestigateUnreadNote = 46,
        PrtsInvestigateNote = 47,
        GameMechanicRead = 48,
        ReadActiveBlackbox = 49,
        ReadLevel = 50,
        FactroyPlacedBuilding = 51,
        EnumMax = 52
    }
    public enum UnlockSystemType // TypeDefIndex: 33602
    {
        Map = 0,
        Inventory = 1,
        Watch = 2,
        ValuableDepot = 3,
        Shop = 4,
        CharTeam = 5,
        Gacha = 51,
        Dungeon = 52,
        BlocMission = 53,
        Mail = 54,
        Wiki = 55,
        PRTS = 56,
        SubmitEther = 57,
        Scan = 58,
        CharUI = 59,
        Friend = 60,
        DailyMission = 61,
        GeneralAbilityBomb = 62,
        GeneralAbilityFluidInteract = 63,
        GeneralAbility = 64,
        SNS = 65,
        EquipTech = 66,
        EquipProduce = 67,
        ItemSubmitRecycle = 68,
        DungeonFactory = 69,
        EnemySpawner = 70,
        FacBuildingPin = 101,
        FacCraftPin = 102,
        FacMode = 103,
        FacTechTree = 104,
        FacOverview = 105,
        FacYieldStats = 106,
        FacConveyor = 107,
        FacTransferPort = 108,
        FacBridge = 109,
        FacSplitter = 110,
        FacMerger = 111,
        FacBUS = 112,
        FacZone = 113,
        FacSystem = 114,
        FacPipe = 115,
        FacPipeSplitter = 116,
        FacPipeConnector = 117,
        FacPipeConverger = 118,
        FacHub = 119,
        ManualCraft = 201,
        ItemUse = 202,
        ItemQuickBar = 203,
        ProductManual = 204,
        Weapon = 251,
        Equip = 252,
        EquipEnhance = 253,
        NormalAttack = 301,
        NormalSkill = 302,
        UltimateSkill = 303,
        TeamSkill = 304,
        ComboSkill = 305,
        TeamSwitch = 306,
        Dash = 307,
        Jump = 308,
        SpaceshipPresentGift = 401,
        SpaceshipManufacturingStation = 402,
        SpaceshipControlCenter = 403,
        SpaceshipSystem = 404,
        SpaceshipGrowCabin = 405,
        SpaceshipShop = 406,
        Settlement = 501,
        RacingDungeon = 601,
        BattleTraining = 602,
        AdventureExpAndLv = 651,
        AdventureBook = 652,
        GuidanceManul = 661,
        AIBark = 670,
        CheckIn = 1113,
        None = 10000000
    }
    public enum AttributeType // TypeDefIndex: 33566
    {
        Level = 0,
        MaxHp = 1,
        Atk = 2,
        Def = 3,
        PhysicalDamageTakenScalar = 4,
        FireDamageTakenScalar = 5,
        PulseDamageTakenScalar = 6,
        CrystDamageTakenScalar = 7,
        Weight = 8,
        CriticalRate = 9,
        CriticalDamageIncrease = 10,
        Hatred = 11,
        NormalAttackRange = 12,
        MoveSpeedScalar = 13,
        TurnRateScalar = 14,
        AttackRate = 15,
        SkillCooldownScalar = 16,
        NormalAttackDamageIncrease = 17,
        HpRecoveryPerSec = 18,
        HpRecoveryPerSecByMaxHpRatio = 19,
        MaxPoise = 20,
        PoiseRecTime = 21,
        MaxUltimateSp = 22,
        DamageTakenScalarWithPoise = 23,
        PoiseDamageTakenScalar = 24,
        PoiseProtectTime = 25,
        PoiseDamageOutputScalar = 26,
        BreakingAttackDamageTakenScalar = 27,
        UltimateSkillDamageIncrease = 28,
        HealOutputIncrease = 29,
        HealTakenIncrease = 30,
        PoiseRecTimeScalar = 31,
        NormalSkillDamageIncrease = 32,
        ComboSkillDamageIncrease = 33,
        KnockDownTimeAddition = 34,
        FireBurstDamageIncrease = 35,
        PulseBurstDamageIncrease = 36,
        CrystBurstDamageIncrease = 37,
        NaturalBurstDamageIncrease = 38,
        Str = 39,
        Agi = 40,
        Wisd = 41,
        Will = 42,
        LifeSteal = 43,
        UltimateSpGainScalar = 44,
        AtbCostAddition = 45,
        SkillCooldownAddition = 46,
        ComboSkillCooldownScalar = 47,
        NaturalDamageTakenScalar = 48,
        IgniteDamageScalar = 49,
        PhysicalDamageIncrease = 50,
        FireDamageIncrease = 51,
        PulseDamageIncrease = 52,
        CrystDamageIncrease = 53,
        NaturalDamageIncrease = 54,
        EtherDamageIncrease = 55,
        FireAbnormalDamageIncrease = 56,
        PulseAbnormalDamageIncrease = 57,
        CrystAbnormalDamageIncrease = 58,
        NaturalAbnormalDamageIncrease = 59,
        EtherDamageTakenScalar = 60,
        DamageToBrokenUnitIncrease = 61,
        Enum = 62
    }
}
