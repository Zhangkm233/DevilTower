using UnityEngine;
using UnityEngine.UI;
using static Grid;

public class UIManager : MonoBehaviour
{
    public Text KeyStat;
    public Text playerStat;
    public Text monsterStat;
    private void Start() {
        monsterStat = this.GetComponentInChildren<Canvas>().transform.GetChild(0).GetComponent<Text>();
        monsterStat.text = " ";
        playerStat = this.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Text>();
        KeyStat = this.GetComponentInChildren<Canvas>().transform.GetChild(2).GetComponent<Text>();
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
        KeyStat.text = 
            "��ͭԿ��: " + GameData.key1.ToString() +
            "\n����Կ��: " + GameData.key2.ToString() +
            "\n�ƽ�Կ��: " + GameData.key3.ToString();
    }
    void updateMonsterStat() {
        if (this.GetComponent<GameManager>().objectClick == null) monsterStat.text = " ";
        if (this.GetComponent<GameManager>().objectClick != null) {
            GameObject objectClicked = this.GetComponent<GameManager>().objectClick;
            Grid gridInMaped = GetComponent<GameManager>().GridInMap;
            if (gridInMaped.type == Grid.GridType.MONSTER) {
                int cDamage = this.GetComponent<GameManager>().caculateDamage((GridMonster)gridInMaped);
                if (cDamage == -1) {
                    monsterStat.text = "����:" + ((GridMonster)gridInMaped).name + " Ԥ���˺�: ???";
                } else {
                    monsterStat.text = "����:" + ((GridMonster)gridInMaped).name + " Ԥ���˺�:" +
                        cDamage.ToString();
                }
                return;
            }
            if (gridInMaped.type == Grid.GridType.BOTTLE) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "СѪƿ �ָ�" + ((GridBottle)gridInMaped).healingPoints.ToString();
                        break;
                    case 2:
                        monsterStat.text = "��Ѫƿ �ָ�" + ((GridBottle)gridInMaped).healingPoints.ToString();
                        break;
                    case 3:
                        monsterStat.text = "��Ѫƿ �ָ�" + ((GridBottle)gridInMaped).healingPoints.ToString();
                        break;
                }
            }
        }
    }
}
