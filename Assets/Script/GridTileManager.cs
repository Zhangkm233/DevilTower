using TMPro;
using UnityEngine;

public class GridTileManager : MonoBehaviour
{
    public Grid mapGrid;
    public Grid.GridType gridType;
    public int mapX;
    public int mapY;

    public void UpdateData() {
        mapGrid = GameData.map[mapX, mapY];
        //Debug.Log(mapGrid.type);
        gridType = mapGrid.type;
        Canvas gridCanvas = this.GetComponentInChildren<Canvas>();

        TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.GridTypeToWord;
        gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.stat.ToString();
    }
}
