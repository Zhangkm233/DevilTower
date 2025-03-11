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
        CORRUPTIONROOT = 2,
        //DEATHPRISONER = 3,
    }
    public EventType eventType;
    public GridEvent(int stat) {
        type = GridType.EVENT;
        eventType = (EventType)stat;
    }
}
