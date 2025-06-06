using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Grid
{
    [System.Serializable]
    [SerializeField]
    public enum GridType{
        BOTTLE = 0,//
        GEM = 1,//
        DOOR = 2,//门 需要钥匙解锁
        MONSTER = 3,//怪兽 点击后进入战斗
        NPC = 4,//可能会有对话或者交易
        EVENT = 5,//特殊物块
        SHOP = 6,//商店
        KEY = 7,//钥匙
        BARRIER = 8,//屏障，不让走
        PORTAL = 9,
    };
    [SerializeField]
    public int stat;
    [SerializeField]
    public GridType type { get; set; }
    [SerializeField]
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
            case "P":
                this.type = GridType.PORTAL;
                break;
            default:
                break;
        }
    }
    public virtual Grid AsEnter() {
        return this;
    }

    public void MoveTo(int x, int y){
        GameData.map[x, y] = this;
        fromX = this.x;
        fromY = this.y;
        this.x = x;
        this.y = y;
    }

    public int x,y;
    public int fromX, fromY;
}
