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
        string temp = "��" + GameData.layer.ToString() + "��\n";
        switch (GameData.layer) {
            case 1:
                temp += "ħŮ����֮��";
                break;
            case 2:
                temp += "�����̻�";
                break;
            case 3:
                temp += "���Ե�ѵ����";
                break;
            case 4:
                temp += "���¼���";
                break;
            case 5:
                temp += "1";
                break;
        }
        layerStat.text = temp;
    }
    void updateCompleteStatSlide() {
        //���½����� ����������
        completeStat.text = GameData.eventEncounter.ToString();
        completeSlider.value = (float)GameData.eventEncounter / 85;
        if (GameData.eventEncounter >= 30) {
            goForgeButton.SetActive(true);
        }
    }
    void updatePlayerKeyText() {
        playerStat.text = 
            "Ѫ��: " + GameData.playerHp.ToString() +
            "\n������: "+ GameData.playerAtk.ToString() +
            "\n������: " + GameData.playerDef.ToString();
        keyStat.text = 
            "��ͭԿ��: " + GameData.key1.ToString() +
            "\n����Կ��: " + GameData.key2.ToString() +
            "\n�ƽ�Կ��: " + GameData.key3.ToString();
        goldStat.text = "�������:" + GameData.gold.ToString();
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
                        " \nԤ���˺�: ???";
                } else {
                    monsterStat.text = gridMonster.name + " " + gridMonster.atk + "/" + gridMonster.def + "/" + gridMonster.hp + 
                        " \nԤ���˺�:" + cDamage.ToString();
                }
                string abilitys = "";
                if (gridMonster.isLostmind) abilitys = abilitys + "ħ�� ";
                if (gridMonster.isCrack) abilitys = abilitys + "���� ";
                if (gridMonster.isFirmness) abilitys = abilitys + "�ᶨ ";
                if (gridMonster.isStalk) abilitys = abilitys + "׷�� ";
                if (gridMonster.isCorruptionOne) abilitys = abilitys + "��ʴ1 ";
                if (gridMonster.isCorruptionTwo) abilitys = abilitys + "��ʴ2 ";
                if (gridMonster.isCorruptionThree) abilitys = abilitys + "��ʴ3 ";
                if (gridMonster.isBoss) abilitys = abilitys + "ͷĿ ";
                monsterAbilityStat.text = abilitys;
                return;
            }
            monsterAbilityStat.text = "";
            if (gridInMaped.type == Grid.GridType.BOTTLE) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "СѪƿ\n�ָ�" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() +"��Ѫ��";
                        break;
                    case 2:
                        monsterStat.text = "��Ѫƿ\n�ָ�" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "��Ѫ��";
                        break;
                    case 3:
                        monsterStat.text = "��Ѫƿ\n�ָ�" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "��Ѫ��";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.GEM) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "������ʯ\n��һ�㹥����";
                        break;
                    case 2:
                        monsterStat.text = "������ʯ\n��һ�������";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.DOOR) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "����ס����ͭ��\nһ����ͭԿ�׽���";
                        break;
                    case 2:
                        monsterStat.text = "����ס�İ�����\nһ�Ѱ���Կ�׽���";
                        break;
                    case 3:
                        monsterStat.text = "����ס�Ļƽ���\nһ�ѻƽ�Կ�׽���";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.KEY) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "һ����ͭԿ��";
                        break;
                    case 2:
                        monsterStat.text = "һ�Ѱ���Կ��";
                        break;
                    case 3:
                        monsterStat.text = "һ�ѻƽ�Կ��";
                        break;
                }
            }
            if (gridInMaped.type == GridType.SHOP) {
                GridShop gridShop = (GridShop)gridInMaped;
                monsterStat.text = "�̵�\n" + gridShop.itemGiveOut + " " + gridShop.itemGiveOutNum + "��\n" +
                    gridShop.itemExchangeFor + " " + gridShop.itemExchangeForNum + "��";
            }
            if (gridInMaped.type == GridType.NPC) {
                monsterStat.text = "NPC ��ʱû��";
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
