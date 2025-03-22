using UnityEngine;

[System.Serializable]
public static class GameData
{
    public static int eventEncounter = 0; //遭遇的事件数量
    public static int layer = 1;
    public static int key1 = 0;
    public static int key2 = 0;
    public static int key3 = 0;
    public static int gold = 0;

    public static int playerHp = 400;
    public static int playerDef = 10;
    public static int playerAtk = 10;
    //铁匠铺升级次数
    public static int forgeTime = 0;

    public static bool hasEncounterBoss = false;
    public static int gridWidth = 6; // 地图宽度
    public static int gridHeight = 15; // 地图高度
    public static Grid[,] map = new Grid[gridWidth,gridHeight];
    public static Vector2Int[,] thisGridComeFrom = new Vector2Int[gridWidth,gridHeight];

    public static int tarotEquip = 0;
    public static bool[] tarotUnlock = new bool[22];
    public static bool[] tarotMissionUnlock = new bool[22];

    public static int saveSlotChoose = 0;

    public static int allGame_DevilsDefeated = 0;
    public static int allGame_Door1Opened = 0;
    public static int allGame_SoulDefeated = 0;
    public static int allGame_GoldGained = 0; 
    public static int allGame_EventEncountered = 0;

    public static bool isEventUsed;
    public static int defeatSHDPJ = 0;
    public static int defeatSHWQJ = 0;
    public static int GetTarotCount(bool[] tarots) {
        int count = 0;
        for (int i = 0;i < tarots.Length;i++) {
            if (tarots[i]) {
                count++;
            }
        }
        return count;
    }
}
