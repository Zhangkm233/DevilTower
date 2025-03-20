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
        STAT,DIALOG,DICTIONARY,SHOP,FORGE,EVENT,TAROT
    };
    public enum sentenceState
    {
        TEXT, EVENT, END,
    };

    public UIState State = UIState.STAT;
    [Header("UI��ʾ���ݶ���")]
    public Text goldStat;
    public Text key1Stat;
    public Text key2Stat;
    public Text key3Stat;
    public Text HpStat;
    public Text AtkStat;
    public Text DefStat;
    public Text monsterStat;
    public Text monsterAbilityStat;
    public Text completeStat;
    public Text layerStat;
    public Slider completeSlider;
    [Header("UI�������")]
    public GameObject forgeMain;
    public GameObject statMain;
    public GameObject dialogMain;
    public GameObject goForgeButton;
    public GameObject shopMain;
    public GameObject buttonMain;
    public GameObject dictionaryMain;
    public GameObject eventMain;
    public GameObject backGroundMain;
    public GameObject tarotMain;
    public GameObject tileMain;
    void Start() {

    }
    
    void Update() {
        updatePlayerKeyText();
        updateGridStat();
        updateCompleteStatSlide();
        updateLayerName();
    }
    public void InitializeUI() {
        monsterStat.text = " ";
        monsterAbilityStat.text = " ";
        goForgeButton.SetActive(false);
        
        StartDialog(0);
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
        HpStat.text = GameData.playerHp.ToString();
        AtkStat.text = GameData.playerAtk.ToString();
        DefStat.text = GameData.playerDef.ToString();
        key1Stat.text = GameData.key1.ToString();
        key2Stat.text = GameData.key2.ToString();
        key3Stat.text = GameData.key3.ToString();
        goldStat.text = GameData.gold.ToString();
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
                if (gridMonster.isStalk) {
                    abilitys = abilitys + "׷��" + gridMonster.stalkTurn +" ";
                }
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
                        monsterStat.text = "������ʯ\n��"+((GridGem)gridInMaped).AddSum + "�㹥����";
                        break;
                    case 2:
                        monsterStat.text = "������ʯ\n��"+((GridGem)gridInMaped).AddSum + "�������";
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
            if (gridInMaped.type == GridType.EVENT) {
                switch (((GridEvent)gridInMaped).eventType) {
                    case GridEvent.EventType.SOULARROW:
                        monsterStat.text = "�л�֮��\n����ֱ������һ������";
                        break;
                    case GridEvent.EventType.SOULGATE:
                        monsterStat.text = "���֮��\n���԰�һ����ǰ�ŵĹ����\n�����ٺ������齻��";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTSAVE:
                        monsterStat.text = "��ֳ֮��\n�����㵱ǰ��һ��Ѫ��";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTLOAD:
                        monsterStat.text = "��ֳ֮��\nȡ��" + ((GridEvent)gridInMaped).hpSave +"Ѫ�����ݻ��������";
                        break;
                }
            }
            if (gridInMaped.type == GridType.BARRIER) {
                monsterStat.text = "�ϰ���\n�޷�ͨ��";
            }
            if (gridInMaped.type == GridType.PORTAL) {
                monsterStat.text = "������\n���͵���һ��";
            }
        }
    }
    [ContextMenu("�л�UIģʽ")]
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
        eventMain.GetComponent<EventManager>().eventType = gridEvent.eventType;
        eventMain.GetComponent<EventManager>().mapX = X;
        eventMain.GetComponent<EventManager>().mapY = Y;
    }
    public void StartDialogBeforeBoss() {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(99);
    }
    public void StartDialogAfterBoss() {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(100);
    }
    public void StartDialog(int dialogStat) {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(dialogStat);
    }
    public void StartTrade(GridShop gridShop,int X,int Y) {
        GoShop();
        shopMain.GetComponent<ShopManager>().UpdateShopData(gridShop,X,Y);
    }
    public void StartForge() {
        if(GameData.eventEncounter < 30) {
            return;
        }
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
    public void GoTarot() {
        GoState(UIState.TAROT);
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
        if (uistate == UIState.EVENT) eventMain.SetActive(true);
        if (uistate != UIState.EVENT) eventMain.SetActive(false);
        if (uistate != UIState.TAROT) {
            backGroundMain.SetActive(true);
            tileMain.SetActive(true);
            tarotMain.SetActive(false);
        }
        if (uistate == UIState.TAROT) {
            backGroundMain.SetActive(false);
            tarotMain.SetActive(true);
            tileMain.SetActive(false);
        }
    }
    public void Cheat() {
        GameData.key1++;
        GameData.key2++;
        GameData.key3++;
        GameData.gold += 1000;
        GameData.playerAtk += 100;
    }
}
