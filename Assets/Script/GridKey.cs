using UnityEngine;

public class GridKey : Grid
{
    public KeyType keyStat;

    public enum KeyType
    {
        GOLD = 3,
        SILVER = 2,
        BRONZE = 1
    }

    public GridKey(int stat){
        this.type = GridType.KEY;
        this.stat = stat;
        keyStat = (KeyType)stat;
    }
}
