using UnityEngine;

public class GridKey : Grid
{
    public KeyType keyStat;

    public enum KeyType
    {
        GOLD,
        SILVER,
        BRONZE
    }

    public GridKey(int stat){
        keyStat = (KeyType)stat;
    }
}
