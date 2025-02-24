using Unity.VisualScripting;
using UnityEngine;

public class Grid
{
    public enum GridType{
        BOTTLE = 0,//
        GEM = 1,//
        DOOR = 2,//门 需要钥匙解锁
        MONSTER = 3,//怪兽 点击后进入战斗
        NPC = 4,//可能会有对话或者交易
        EVENT = 5,//事件
        SHOP = 6,//商店

        BARRIER = 999,//屏障，不让走
    };

    public int stat;
    public GridType type { get; set; }
    public Grid(int stat,GridType type) {
        this.stat = stat;
        this.type = type;
    }

    public Grid() {

    }

    //M怪物 X封锁 D门 G宝石 B血瓶 K钥匙 N NPC S商人
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
