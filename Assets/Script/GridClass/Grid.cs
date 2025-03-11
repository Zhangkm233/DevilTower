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
        EVENT = 5,//特殊物块
        SHOP = 6,//商店
        KEY = 7,//钥匙

        BARRIER = 999,//屏障，不让走
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

    //M怪物 X封锁 D门 G宝石 B血瓶 K钥匙 N NPC S商人
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
