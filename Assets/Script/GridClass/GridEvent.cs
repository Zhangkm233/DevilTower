using UnityEngine;

public class GridEvent : Grid
{
    /*�������
    ���;�л�֮������ֱ������һ�����0
    ���;���֮�ſ�������Ұ�һ����ǰ�ŵĹ���ͺ����ٺ������齻����1
    ���;��ֳ֮��;�״ν������Դ����㵱ǰ��һ��Ѫ�����ٴν���ȡ��Ѫ�����ݻ�������ӣ�Ȼ������¼���2
    ���;������ͽ;���Ի��һ�����棬���ݹ������Ѫ��/��ù���������Ѫ��/��ô���Կ�ף�3*/
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
