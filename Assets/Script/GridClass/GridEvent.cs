using UnityEngine;

public class GridEvent : Grid
{
    /*�������
    ��飺�л�֮������ֱ������һ�����0
    ��飺���֮�ſ�������Ұ�һ����ǰ�ŵĹ���ͺ����ٺ������齻����1
    ��飺��ֳ֮�����״ν������Դ����㵱ǰ��һ��Ѫ�����ٴν���ȡ��Ѫ�����ݻ�������ӣ�Ȼ������¼���2
    ��飺������ͽ�����Ի��һ�����棬���ݹ������Ѫ��/��ù���������Ѫ��/��ô���Կ�ף�3*/
    public enum EventType
    {
        SOULARROW = 0,
        SOULGATE = 1,
        CORRUPTIONROOTSAVE = 2,
        CORRUPTIONROOTLOAD = 3,
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
