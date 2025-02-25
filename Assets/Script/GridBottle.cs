using UnityEngine;

public class GridBottle : Grid
{
    private static readonly int[,] healingPointsTable = new int[3,4] {
        { 50, 100, 150, 200 },
        { 100, 175, 250, 400 },
        { 200, 400, 600, 1000 }
    };

    

    public enum BottleSizeType{
        SMALL,
        MIDDLE,
        BIG
    }

    public BottleSizeType bottleSize;
    public int healingPoints;

    public GridBottle(int stat, int layer){
        bottleSize = (BottleSizeType)stat;
        healingPoints = healingPointsTable[(int)bottleSize, layer];
    }

    public GridBottle(BottleSizeType size, int layer){
        bottleSize = size;
        healingPoints = healingPointsTable[(int)size, layer];
    }
}
