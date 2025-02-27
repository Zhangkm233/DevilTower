using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridTileManager : MonoBehaviour
{
    public Grid mapGrid;
    public Grid.GridType gridType;
    public int mapX;
    public int mapY;
    public Text monsterStat;
    public GameObject gameManagerObject;
    private void Start() {
        //monsterStat = gameManagerObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
    }
    private void OnMouseEnter() {
       /*if (gridType == Grid.GridType.MONSTER) {
            int cDamage = gameManagerObject.GetComponent<GameManager>().caculateDamage((GridMonster)mapGrid);
            if (cDamage == -1) {
                monsterStat.text = "Ãû×Ö:" + ((GridMonster)mapGrid).name + " Ô¤¼ÆÉËº¦: ???"; 
            } else {
                monsterStat.text = "Ãû×Ö:" + ((GridMonster)mapGrid).name + " Ô¤¼ÆÉËº¦:" +
                    cDamage.ToString();
            }
        }*/
    }
    private void OnMouseExit() {
        /*if (monsterStat.text != null) {
            monsterStat.text = null;
        }*/
    }
    public void UpdateData() {
        if (GameData.map[mapX,mapY] != null) {
            mapGrid = GameData.map[mapX,mapY];
            gridType = mapGrid.type;
            UpdateText();
        } else {
            mapGrid = null;
            gridType = Grid.GridType.BARRIER;
            Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
            TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
            gridtext.text = " ";
            gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
            gridtext.text = " ";
        }
    }
    public void UpdateText() {
        Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
        TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.GridTypeToWord;
        gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.stat.ToString();
    }
}
