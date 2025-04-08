using UnityEngine;

public class GridBottle : Grid
{
    public static readonly int[,] healingPointsTable = new int[3,4] {
        { 25, 50, 75, 200 },
        { 100, 125, 200, 400 },
        { 200, 225, 300, 1000 }
    };

    public enum BottleSizeType{
        SMALL = 1,
        MIDDLE = 2,
        BIG = 3
    }

    public BottleSizeType bottleSize;
    public int healingPoints;

    public GridBottle(int stat) {
        this.type = GridType.BOTTLE;
        this.stat = stat;
        bottleSize = (BottleSizeType)stat;
        healingPoints = healingPointsTable[(int)bottleSize - 1,GameData.layer - 1];
    }
    public GridBottle(int stat, int layer) {
        this.type = GridType.BOTTLE;
        this.stat = stat;
        bottleSize = (BottleSizeType)stat;
        healingPoints = healingPointsTable[(int)bottleSize - 1, layer - 1];
    }

    public GridBottle(BottleSizeType size, int layer) {
        this.type = GridType.BOTTLE;
        // this.stat = stat;
        bottleSize = size;
        healingPoints = healingPointsTable[(int)size - 1, layer - 1];
    }
}
