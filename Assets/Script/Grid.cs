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
        EVENT = 5,//�¼�
        SHOP = 6,//�̵�

        BARRIER = 999,//���ϣ�������
    };

    public int stat;
    public GridType type { get; set; }
    public Grid(int stat,GridType type) {
        this.stat = stat;
        this.type = type;
    }

    public Grid() {

    }

    //M���� X���� D�� G��ʯ BѪƿ KԿ�� N NPC S����
    public Grid(string type,int stat) {
        this.stat = stat;
        switch (type) {
            case "B":
                this.type = GridType.BOTTLE;
                break;
            case "X":
                this.type = GridType.BARRIER;
                break;
            case "D":
                this.type = GridType.DOOR;
                break;
            case "N":
                this.type = GridType.NPC;
                break;
            case "G":
                this.type = GridType.GEM;
                break;
            case "S":
                this.type = GridType.SHOP;
                break;
            default:
                this.type = GridType.BARRIER;
                break;
        }
    }
    public virtual Grid AsEnter() {
        return this;
    }
}
