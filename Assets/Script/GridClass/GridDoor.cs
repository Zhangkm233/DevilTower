using UnityEngine;

public class GridDoor : Grid
{
    public DoorType doorStat;

    public enum DoorType
    {
        GOLD = 3,
        SILVER = 2,
        BRONZE = 1
    }

    public GridDoor(int stat){
        this.type = GridType.DOOR;
        this.stat = stat;
        doorStat = (DoorType)stat;
    }
}
