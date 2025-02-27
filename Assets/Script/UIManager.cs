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
            "ÑªÁ¿: " + GameData.playerHp.ToString() +
            "\n¹¥»÷Á¦: "+ GameData.playerAtk.ToString() +
            "\n·ÀÓùÁ¦: " + GameData.playerDef.ToString();
        KeyStat.text = 
            "ÇàÍ­Ô¿³×: " + GameData.key1.ToString() +
            "\n°×ÒøÔ¿³×: " + GameData.key2.ToString() +
            "\n»Æ½ðÔ¿³×: " + GameData.key3.ToString();
    }
    void updateMonsterStat() {
        if (this.GetComponent<GameManager>().objectClick == null) monsterStat.text = " ";
        if (this.GetComponent<GameManager>().objectClick != null) {
            GameObject objectClicked = this.GetComponent<GameManager>().objectClick;
            Grid gridInMaped = GetComponent<GameManager>().GridInMap;
            if (gridInMaped.type == Grid.GridType.MONSTER) {
                int cDamage = this.GetComponent<GameManager>().caculateDamage((GridMonster)gridInMaped);
                if (cDamage == -1) {
                    monsterStat.text = "Ãû×Ö:" + ((GridMonster)gridInMaped).name + " Ô¤¼ÆÉËº¦: ???";
                } else {
                    monsterStat.text = "Ãû×Ö:" + ((GridMonster)gridInMaped).name + " Ô¤¼ÆÉËº¦:" +
                        cDamage.ToString();
                }
                return;
            }
            if (gridInMaped.type == Grid.GridType.BOTTLE) {
                switch (gridInMaped.stat) {
                    case 1:
                        monsterStat.text = "Ð¡ÑªÆ¿ »Ö¸´" + ((GridBottle)gridInMaped).healingPoints.ToString();
                        break;
                    case 2:
                        monsterStat.text = "ÖÐÑªÆ¿ »Ö¸´" + ((GridBottle)gridInMaped).healingPoints.ToString();
                        break;
                    case 3:
                        monsterStat.text = "´óÑªÆ¿ »Ö¸´" + ((GridBottle)gridInMaped).healingPoints.ToString();
                        break;
                }
            }
        }
    }
}
