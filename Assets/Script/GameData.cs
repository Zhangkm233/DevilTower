using UnityEngine;

[System.Serializable]
public static class GameData
{
    public static int eventEncounter = 0; //遭遇的事件数量
    public static int layer = 1;
    public static int key1 = 0;
    public static int key2 = 0;
    public static int key3 = 0;

    public static int playerHp = 10;
    public static int playerDef = 10;
    public static int playerAtk = 10;

    public static int gridWidth = 6; // 地图宽度
    public static int gridHeight = 15; // 地图高度
    public static Grid[,] map = new Grid[gridWidth,gridHeight];
}
