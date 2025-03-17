using NUnit.Framework.Constraints;
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
        STAT,DIALOG,DICTIONARY,SHOP,FORGE,EVENT
    };
    public enum sentenceState
    {
        TEXT, EVENT, END,
    };

    public UIState State = UIState.STAT;
    [Header("UI显示数据对象")]
    public Text goldStat;
    public Text keyStat;
    public Text playerStat;
    public Text monsterStat;
    public Text monsterAbilityStat;
    public Text completeStat;
    public Text layerStat;
    public Slider completeSlider;
    [Header("UI主体对象")]
    public GameObject forgeMain;
    public GameObject statMain;
    public GameObject dialogMain;
    public GameObject goForgeButton;
    public GameObject shopMain;
    public GameObject buttonMain;
    public GameObject dictionaryMain;
    public GameObject EventMain;

    void Start() {
        statMain = this.transform.GetChild(0).gameObject;
        dialogMain = this.transform.GetChild(1).gameObject;

        monsterStat = this.GetComponentInChildren<Canvas>().transform.GetChild(0).GetComponent<Text>();
        monsterAbilityStat = this.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Text>();
        playerStat = this.GetComponentInChildren<Canvas>().transform.GetChild(2).GetComponent<Text>();
        keyStat = this.GetComponentInChildren<Canvas>().transform.GetChild(3).GetComponent<Text>();

        monsterStat.text = " ";
        monsterAbilityStat.text = " ";
        goForgeButton.SetActive(false);
        GoStat();
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
                if (gridMonster.isStalk) {
                    abilitys = abilitys + "追猎" + gridMonster.stalkTurn +" ";
                }
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
                        monsterStat.text = "攻击宝石\n加"+((GridGem)gridInMaped).AddSum + "点攻击力";
                        break;
                    case 2:
                        monsterStat.text = "防御宝石\n加"+((GridGem)gridInMaped).AddSum + "点防御力";
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
            if (gridInMaped.type == GridType.EVENT) {
                switch (((GridEvent)gridInMaped).eventType) {
                    case GridEvent.EventType.SOULARROW:
                        monsterStat.text = "拘魂之箭\n可以直接消灭一个怪物";
                        break;
                    case GridEvent.EventType.SOULGATE:
                        monsterStat.text = "灵魂之门\n可以把一个最前排的怪物和\n后面再后面的物块交换";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTSAVE:
                        monsterStat.text = "腐殖之根\n存入你当前的一半血量";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTLOAD:
                        monsterStat.text = "腐殖之根\n取出" + ((GridEvent)gridInMaped).hpSave +"血量并摧毁这个格子";
                        break;
                }
            }
            if (gridInMaped.type == GridType.BARRIER) {
                monsterStat.text = "障碍物\n无法通过";
            }
            if (gridInMaped.type == GridType.PORTAL) {
                monsterStat.text = "传送门\n传送到下一层";
            }
        }
    }
    [ContextMenu("切换UI模式")]
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
    public void StartEvent(GridEvent gridEvent,int X,int Y) {
        GoEvent();
        EventMain.GetComponent<EventManager>().eventType = gridEvent.eventType;
        EventMain.GetComponent<EventManager>().mapX = X;
        EventMain.GetComponent<EventManager>().mapY = Y;
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
    public void GoDictionary() {
        GoState(UIState.DICTIONARY);
    }
    public void GoEvent() {
        GoState(UIState.EVENT);
    }
    public void GoState(UIState uistate) {
        State = uistate;
        if (uistate == UIState.DIALOG) dialogMain.SetActive(true);
        if (uistate != UIState.DIALOG) dialogMain.SetActive(false);
        if (uistate == UIState.STAT) {
            statMain.SetActive(true);
            buttonMain.SetActive(true);
            dictionaryMain.SetActive(true);
        }
        if (uistate != UIState.STAT) {
            statMain.SetActive(false);
            buttonMain.SetActive(false);
            dictionaryMain.SetActive(false);
        }
        if (uistate == UIState.SHOP) shopMain.SetActive(true);
        if (uistate != UIState.SHOP) shopMain.SetActive(false);
        if (uistate == UIState.FORGE) forgeMain.SetActive(true);
        if (uistate != UIState.FORGE) forgeMain.SetActive(false);
        if (uistate == UIState.DICTIONARY) dictionaryMain.SetActive(true);
        if (uistate == UIState.EVENT) EventMain.SetActive(true);
        if (uistate != UIState.EVENT) EventMain.SetActive(false);
    }
    public void Cheat() {
        GameData.key1++;
        GameData.key2++;
        GameData.key3++;
        GameData.gold += 1000;
        GameData.playerAtk += 100;
    }
}
