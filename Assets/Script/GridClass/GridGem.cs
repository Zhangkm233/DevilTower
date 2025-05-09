using UnityEngine;

public class GridGem : Grid
{
    public int AddSum;
    public enum GemType{
        ATK = 1,
        DEF = 2,
    }

    public GemType gemType;
    public GridGem(int stat) {
        this.type = GridType.GEM;
        this.stat = stat;
        this.gemType = (GemType)stat;
        AddSum = GameData.layer switch {
            1 => 0,
            2 => 1,
            3 => 2,
            4 => 4,
            5 => 7,
            6 => 10,
            _ => 0
        };
    }
}
