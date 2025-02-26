using TMPro;
using UnityEngine;

public class GridTileManager : MonoBehaviour
{
    public Grid mapGrid;
    public Grid.GridType gridType;
    public int mapX;
    public int mapY;

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
