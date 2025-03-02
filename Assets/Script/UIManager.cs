using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Grid;

public class UIManager : MonoBehaviour
{
    public enum UIState {
        STAT,DIALOG,DICTIONARY,
    };
    public UIState State = UIState.STAT;
    public Text keyStat;
    public Text playerStat;
    public Text monsterStat;
    public Text monsterAbilityStat;
    public Text dialogName;
    public Text dialogText;
    public GameObject statMain;
    public GameObject dialogMain;
    public string dialogTitle;
    public int sentenceNumber = -1;
    public enum sentenceState {
        TEXT,EVENT,
    }
    public sentenceState[] sentenceStates;
    public string[] sentenceNames;
    public string[] sentenceTexts;

    private void Start() {
        statMain = this.transform.GetChild(0).gameObject;
        dialogMain = this.transform.GetChild(1).gameObject;
        monsterStat = this.GetComponentInChildren<Canvas>().transform.GetChild(0).GetComponent<Text>();
        monsterAbilityStat = this.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Text>();
        playerStat = this.GetComponentInChildren<Canvas>().transform.GetChild(2).GetComponent<Text>();
        keyStat = this.GetComponentInChildren<Canvas>().transform.GetChild(3).GetComponent<Text>();
        dialogName = this.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        dialogText = this.transform.GetChild(1).GetChild(2).GetComponent<Text>();

        monsterStat.text = " ";
        monsterAbilityStat.text = " ";
        dialogName.text = "��";
        dialogText.text = "���Ϲ�������ͷ��\n��ҡͷ��";
        dialogMain.SetActive(false);
        ReadDialog(1);
    }
    private void Update() {
        updatePlayerKeyText();
        updateMonsterStat();
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
                    monsterStat.text = "����:" + gridMonster.name + " Ԥ���˺�: ???";
                } else {
                    monsterStat.text = "����:" + gridMonster.name + " Ԥ���˺�:" +
                        cDamage.ToString();
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
        }
    }
    public void ChangeState() {
        switch (State) {
            case UIState.STAT:
                State = UIState.DIALOG;
                dialogMain.SetActive(true);
                statMain.SetActive(false);
                break;
            case UIState.DIALOG:
                State = UIState.DICTIONARY;
                dialogMain.SetActive(false);
                statMain.SetActive(false);
                break;
            case UIState.DICTIONARY:
                State = UIState.STAT;
                dialogMain.SetActive(false);
                statMain.SetActive(true);
                break;
        }
    }
    public void ReadDialog(int dialogNumber) {
        //dialogTitle = "Assets/Resources/dialog" + dialogNumber +".txt";
        dialogTitle = "Assets/Resources/dialog1.txt";
        Debug.Log(dialogTitle);
        string[] lines = File.ReadAllLines(dialogTitle);
        Debug.Log(lines[0]);
        Debug.Log(lines[0].Split(";")[2]);
        string[] sentenceNames = new string[lines.Length];
        string[] sentenceTexts = new string[lines.Length];
        for (int i = 0;i < lines.Length;i++) {
            Debug.Log(i);
            //sentenceStates[i] = (sentenceState)Enum.Parse(typeof(sentenceState),lines[i].Split(';')[0]);
            sentenceNames[i] = "a";
            sentenceNames[i] = (lines[i].Split(";"))[1];
            sentenceTexts[i] = lines[i].Split(";")[2];
        }
    }
    public void NextSentence() {
        //Ϊʲô��bug

        if (sentenceNames[sentenceNumber + 1] == null) {
            sentenceNumber = 0;
            return;
        }
        sentenceNumber++;
        dialogName.text = sentenceNames[sentenceNumber];
        dialogText.text = sentenceTexts[sentenceNumber];
    }
}
