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
    }
    private void OnMouseEnter() {
    }
    private void OnMouseExit() {
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
        transform.name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;
        Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
        TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.GridTypeToWord;
        gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.stat.ToString();
    }
}
