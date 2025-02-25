using UnityEngine;

public class GridDoor : Grid
{
    public DoorType doorStat;

    public enum DoorType
    {
        GOLD,
        SILVER,
        BRONZE
    }

    public GridDoor(int stat){
        doorStat = (DoorType)stat;
    }
}
