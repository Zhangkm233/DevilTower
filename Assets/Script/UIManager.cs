using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Grid;

public class UIManager : MonoBehaviour
{
    public enum UIState {
        STAT,DIALOG,DICTIONARY,SHOP,FORGE
    };
    public enum sentenceState
    {
        TEXT, EVENT, END,
    };

    public UIState State = UIState.STAT;
    public Text goldStat;
    public Text keyStat;
    public Text playerStat;
    public Text monsterStat;
    public Text monsterAbilityStat;
    public Text completeStat;
    public Text layerStat;
    public Slider completeSlider;
    public GameObject forgeMain;
    public GameObject statMain;
    public GameObject dialogMain;
    public GameObject goForgeButton;
    public GameObject shopMain;

    void Start() {
        statMain = this.transform.GetChild(0).gameObject;
        dialogMain = this.transform.GetChild(1).gameObject;

        monsterStat = this.GetComponentInChildren<Canvas>().transform.GetChild(0).GetComponent<Text>();
        monsterAbilityStat = this.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Text>();
        playerStat = this.GetComponentInChildren<Canvas>().transform.GetChild(2).GetComponent<Text>();
        keyStat = this.GetComponentInChildren<Canvas>().transform.GetChild(3).GetComponent<Text>();

        monsterStat.text = " ";
        monsterAbilityStat.text = " ";
        dialogMain.SetActive(false);
        goForgeButton.SetActive(false);
        shopMain.SetActive(false);
        forgeMain.SetActive(false);
    }
    void Update() {
        updatePlayerKeyText();
        updateGridStat();
        updateCompleteStatSlide();
        updateLayerName();
    }
    void updateLayerName() {
        string temp = "第" + GameData.layer.ToString() + "层\n";
        switch (GameData.layer) {
            case 1:
                temp += "魔女隐居之地";
                break;
            case 2:
                temp += "月亮教会";
                break;
            case 3:
                temp += "狂猎的训练场";
                break;
            case 4:
                temp += "地下监狱";
                break;
            case 5:
                temp += "1";
                break;
        }
        layerStat.text = temp;
    }
    void updateCompleteStatSlide() {
        //更新进度条 更新铁匠铺
        completeStat.text = GameData.eventEncounter.ToString();
        completeSlider.value = (float)GameData.eventEncounter / 85;
        if (GameData.eventEncounter >= 30) {
            goForgeButton.SetActive(true);
        }
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
        goldStat.text = "金币数量:" + GameData.gold.ToString();
    }
    void updateGridStat() {
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
                    monsterStat.text = gridMonster.name + " " + gridMonster.atk + "/" + gridMonster.def +"/" + gridMonster.hp + 
                        " \n预计伤害: ???";
                } else {
                    monsterStat.text = gridMonster.name + " " + gridMonster.atk + "/" + gridMonster.def + "/" + gridMonster.hp + 
                        " \n预计伤害:" + cDamage.ToString();
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
            monsterAbilityStat.text = "";
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
            if (gridInMaped.type == GridType.SHOP) {
                GridShop gridShop = (GridShop)gridInMaped;
                monsterStat.text = "商店\n" + gridShop.itemGiveOut + " " + gridShop.itemGiveOutNum + "个\n" +
                    gridShop.itemExchangeFor + " " + gridShop.itemExchangeForNum + "个";
            }
            if (gridInMaped.type == GridType.NPC) {
                monsterStat.text = "NPC 暂时没用";
            }
        }
    }
    public void ChangeState() {
        switch (State) {
            case UIState.STAT:
                GoDialog();
                break;
            case UIState.DIALOG:
                GoStat();
                break;
        }
    }
    public void StartTrade(GridShop gridShop,int X,int Y) {
        GoShop();
        shopMain.GetComponent<ShopManager>().UpdateShopData(gridShop,X,Y);
    }
    public void StartForge() {
        GoForge();
        forgeMain.GetComponent<ForgeManager>().initializePrice();
    }
    public void GoDialog() {
        GoState(UIState.DIALOG);
    }
    public void GoStat() {
        GoState(UIState.STAT);
    }
    public void GoShop() {
        GoState(UIState.SHOP);
    }

    public void GoForge() {
        GoState(UIState.FORGE);
    }
    public void GoState(UIState uistate) {
        State = uistate;
        if (uistate == UIState.DIALOG) dialogMain.SetActive(true);
        if (uistate != UIState.DIALOG) dialogMain.SetActive(false);
        if (uistate == UIState.STAT) statMain.SetActive(true);
        if (uistate != UIState.STAT) statMain.SetActive(false);
        if (uistate == UIState.SHOP) shopMain.SetActive(true);
        if (uistate != UIState.SHOP) shopMain.SetActive(false);
        if (uistate == UIState.FORGE) forgeMain.SetActive(true);
        if (uistate != UIState.FORGE) forgeMain.SetActive(false);
    }
    public void Cheat() {
        GameData.key1++;
        GameData.key2++;
        GameData.key3++;
        GameData.gold += 10;
    }
}
