using UnityEngine;
using UnityEngine.UI;
using static Grid;

public class UIManager : MonoBehaviour
{
    public Text keyStat;
    public Text playerStat;
    public Text monsterStat;
    public Text monsterAbilityStat;
    private void Start() {
        monsterStat = this.GetComponentInChildren<Canvas>().transform.GetChild(0).GetComponent<Text>();
        monsterStat.text = " ";
        monsterAbilityStat = this.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Text>();
        monsterAbilityStat.text = " ";
        playerStat = this.GetComponentInChildren<Canvas>().transform.GetChild(2).GetComponent<Text>();
        keyStat = this.GetComponentInChildren<Canvas>().transform.GetChild(3).GetComponent<Text>();
    }
    private void Update() {
        updatePlayerKeyText();
        updateMonsterStat();
    }
    void updatePlayerKeyText() {
        playerStat.text = 
            "血量: " + GameData.playerHp.ToString() +
            "\n攻击力: "+ GameData.playerAtk.ToString() +
            "\n防御力: " + GameData.playerDef.ToString();
        keyStat.text = 
            "青铜钥匙: " + GameData.key1.ToString() +
            "\n白银钥匙: " + GameData.key2.ToString() +
            "\n黄金钥匙: " + GameData.key3.ToString();
    }
    void updateMonsterStat() {
        if (this.GetComponent<GameManager>().objectClick == null) {
            monsterStat.text = " ";
            monsterAbilityStat.text = " ";
        }
        if (this.GetComponent<GameManager>().objectClick != null) {
            GameObject objectClicked = this.GetComponent<GameManager>().objectClick;
            if (GetComponent<GameManager>().GridInMap == null) return;
            Grid gridInMaped = GetComponent<GameManager>().GridInMap;
            if (gridInMaped.type == Grid.GridType.MONSTER) {
                GridMonster gridMonster = (GridMonster)gridInMaped;
                int cDamage = this.GetComponent<GameManager>().CaculateDamage(gridMonster);
                if (cDamage == -1) {
                    monsterStat.text = "名字:" + gridMonster.name + " 预计伤害: ???";
                } else {
                    monsterStat.text = "名字:" + gridMonster.name + " 预计伤害:" +
                        cDamage.ToString();
                }
                string abilitys = "";
                if (gridMonster.isLostmind) abilitys = abilitys + "魔心 ";
                if (gridMonster.isCrack) abilitys = abilitys + "碎裂 ";
                if (gridMonster.isFirmness) abilitys = abilitys + "坚定 ";
                if (gridMonster.isStalk) abilitys = abilitys + "追猎 ";
                if (gridMonster.isCorruptionOne) abilitys = abilitys + "腐蚀1 ";
                if (gridMonster.isCorruptionTwo) abilitys = abilitys + "腐蚀2 ";
                if (gridMonster.isCorruptionThree) abilitys = abilitys + "腐蚀3 ";
                if (gridMonster.isBoss) abilitys = abilitys + "头目 ";
                monsterAbilityStat.text = abilitys;
                return;
            }
            if (gridInMaped.type == Grid.GridType.BOTTLE) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "小血瓶\n恢复" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() +"点血量";
                        break;
                    case 2:
                        monsterStat.text = "中血瓶\n恢复" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "点血量";
                        break;
                    case 3:
                        monsterStat.text = "大血瓶\n恢复" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "点血量";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.GEM) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "攻击宝石\n加一点攻击力";
                        break;
                    case 2:
                        monsterStat.text = "防御宝石\n加一点防御力";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.DOOR) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "被锁住的青铜门\n一把青铜钥匙解锁";
                        break;
                    case 2:
                        monsterStat.text = "被锁住的白银门\n一把白银钥匙解锁";
                        break;
                    case 3:
                        monsterStat.text = "被锁住的黄金门\n一把黄金钥匙解锁";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.KEY) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "一把青铜钥匙";
                        break;
                    case 2:
                        monsterStat.text = "一把白银钥匙";
                        break;
                    case 3:
                        monsterStat.text = "一把黄金钥匙";
                        break;
                }
            }
        }
    }
}
