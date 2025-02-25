using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    public GameObject gridPrefab;
    public Grid[,] map = new Grid[GameData.gridWidth,3];
    public Sprite gridSprite;

    public void DrawThreeGrid() {
        for (int i = 0;i < GameData.gridWidth;i++) {
            for (int j = 0;j < 3;j++) {
                GameObject newgrid = Instantiate(gridPrefab);
                Grid mapGrid = GameData.map[i,GameData.gridHeight - j - 1];
                newgrid.GetComponent<GridTileManager>().gridType = mapGrid.type;
                newgrid.GetComponent<GridTileManager>().mapX = i;
                newgrid.GetComponent<GridTileManager>().mapY = GameData.gridHeight - j - 1;
                newgrid.name = mapGrid.GridTypeToWord + " " +  i + " " + j;
                newgrid.transform.position = new Vector3((i*2)-5,(j*2)-1,0);
                Canvas gridCanvas = newgrid.GetComponentInChildren<Canvas>();

                //更改格子上的文字
                TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
                gridtext.text = mapGrid.GridTypeToWord;
                gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
                gridtext.text = mapGrid.stat.ToString();
            }
        }
    }

    public void FillGrid() {

    }
}
