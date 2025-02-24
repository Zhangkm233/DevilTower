using UnityEngine;

[System.Serializable]
public static class GameData
{
    public static int layer = 1;
    public static int hp = 0;
    public static int def = 0;
    public static int key1 = 0;
    public static int key2 = 0;
    public static int key3 = 0;

    public static int gridWidth = 6; // ��ͼ���
    public static int gridHeight = 15; // ��ͼ�߶�
    public static Grid[,] map = new Grid[gridWidth,gridHeight];
}
