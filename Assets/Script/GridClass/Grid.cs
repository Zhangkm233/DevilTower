using Unity.VisualScripting;
using UnityEngine;

public class Grid
{
    public enum GridType{
        BOTTLE = 0,//
        GEM = 1,//
        DOOR = 2,//�� ��ҪԿ�׽���
        MONSTER = 3,//���� ��������ս��
        NPC = 4,//���ܻ��жԻ����߽���
        EVENT = 5,//�������
        SHOP = 6,//�̵�
        KEY = 7,//Կ��

        BARRIER = 999,//���ϣ�������
    };

    public int stat;
    public GridType type { get; set; }
    public string GridTypeToWord;
    public Grid(int stat,GridType type) {
        this.stat = stat;
        this.type = type;
    }

    public Grid() {

    }

    //M���� X���� D�� G��ʯ BѪƿ KԿ�� N NPC S����
    public Grid(string type,int stat) {
        GridTypeToWord = type;
        this.stat = stat;
        switch (type) {
            case "X":
                this.type = GridType.BARRIER;
                break;
            case "N":
                this.type = GridType.NPC;
                break;
            case "S":
                this.type = GridType.SHOP;
                break;
            default:
                break;
        }
    }
    public virtual Grid AsEnter() {
        return this;
    }
}
