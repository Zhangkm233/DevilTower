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
    public SpriteScriptObject layer1sprites;
    public void InitialData() {
        this.gameObject.layer = 6;
        gameManagerObject = GameObject.Find("GameManager"); 
        name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;
    }
    public void UpdateData() {
        if (GameData.map[mapX,mapY] != null) {
            mapGrid = GameData.map[mapX,mapY];
            gridType = mapGrid.type;
            //UpdateText();
            //临时代码
            transform.name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;
            Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
            TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
            gridtext.text = " ";
            gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
            gridtext.text = " ";

            UpdateSprite();
        } else {
            //没东西就清空
            mapGrid = null;
            gridType = Grid.GridType.BARRIER;
            Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
            TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
            gridtext.text = " ";
            gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
            gridtext.text = " ";

            this.gameObject.GetComponent<SpriteRenderer>().sprite = layer1sprites.spriteData[8].sprites[0];
        }
    }
    public void UpdateText() {
        transform.name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;
        Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
        TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.GridTypeToWord;
        gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.stat.ToString();
    }
    public void UpdateSprite() {
        try {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = layer1sprites.spriteData[(int)gridType].sprites[mapGrid.stat - 1];
        } catch (System.Exception e) {
            Debug.Log("Error: " + mapGrid.GridTypeToWord + " " + mapGrid.stat + e);
        }
    }
}
