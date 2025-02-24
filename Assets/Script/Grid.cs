using UnityEngine;

public class Grid
{
    public enum GridType{
        EMPTY = 0,
        ITEM = 1,
        DOOR = 2,
        MONSTER = 3,
        NPC = 4,
        EVENT = 5,
        BARRIER = 999,
    };
    public GridType type { get; set; }
}
