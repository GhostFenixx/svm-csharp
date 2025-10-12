namespace Greed.Models.PMCData
{
    public class PMC
    {

        public bool NameOverride { get; set; }
        public bool ForceCustomWaves { get; set; }
        public int CustomWaveChance { get; set; } = 100;
        public PMCChance PMCChance { get; set; }
        public int LevelUpMargin { get; set; } = 10;
        public int LevelDownMargin { get; set; } = 70;
        public string PMCNameList { get; set; } = "Lacyway\r\nTrippyone\r\nssh\r\nDeadLeaves\r\nArchangel\r\nTheSparta\r\nSeion\r\nDOKDOR\r\nguidot\r\nGhostFenixx\r\nJanuary\r\nMorgan\r\nNimbul\r\nShiro\r\nTallan\r\nEkuland\r\nHustleHarder\r\nMissingTarget\r\nMrElmoEN\r\nNekoKami\r\nuprior\r\nVenican\r\nShynd\r\nCWX\r\nEreshkigal\r\nSenko\r\nChomp\r\nsptlaggy\r\nSerWolfik\r\nVolcano\r\nNexus4880\r\nFireHawk\r\nZ3R0\r\nRakTheGoose\r\nMorgan\r\nAssAssIn\r\nTabi\r\nG10rgos\r\nDaveyB0y\r\nFortis\r\nolli991\r\nKain187\r\nGamesB4Gains\r\nKiriko\r\nBiddinWar\r\n루퍼\r\n고라니\r\ncelebrutu\r\nogruby\r\nKWJimWails\r\nMaxomatic458\r\nNickMillion\r\nzartabulon\r\nzedramus\r\nslickboi\r\nNejurnia\r\nJuncker\r\nQuikstar\r\ntarkin\r\nTraveler";
        public bool NamesEnable { get; set; }
        public bool ChancesEnable { get; set; }
        public int PMCRatio { get; set; } = 50;
        public AItoPMC AItoPMC { get; set; }
        public bool EnableConvert { get; set; }
        public bool DisableLowLevelPMC { get; set; }
        public bool LootableMelee { get; set; }
        public bool EnablePMC { get; set; }
        public PMC()
        {
            PMCChance = new PMCChance();
            AItoPMC = new AItoPMC();
        }
    }
}
