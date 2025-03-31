using UnityEngine;

public class GridEvent : Grid
{
    /*特殊物块
    物块;拘魂之箭可以直接消灭一个怪物。0
    物块;灵魂之门可以让玩家把一个最前排的怪物和后面再后面的物块交换。1
    物块;腐殖之根;首次交互可以存入你当前的一半血量，再次交互取出血量并摧毁这个格子，然后完成事件。2
    物块;死亡囚徒;可以获得一次增益，根据攻防获得血量/获得攻防和少量血量/获得大量钥匙，3*/
    public enum EventType
    {
        SOULARROW = 1,
        SOULGATE = 2,
        CORRUPTIONROOTSAVE = 3,
        CORRUPTIONROOTLOAD = 4,
        BOOM = 5,
        NULL = 99
    }
    public int hpSave;
    public EventType eventType;
    public GridEvent(int stat) {
        this.stat = stat;
        type = GridType.EVENT;
        eventType = (EventType)stat;
    }
}
