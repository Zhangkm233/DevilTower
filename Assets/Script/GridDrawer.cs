using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject TilesParent;
    public Grid[,] map = new Grid[GameData.gridWidth,3];
    public Sprite gridSprite;
    private void Start() {
        //DrawThreeGrid();
    }
    public void DrawThreeGrid() {
        for (int j = 0;j < 3;j++) {
            for (int i = 0;i < GameData.gridWidth;i++) {
                GameObject newgrid = Instantiate(gridPrefab);
                Grid mapGrid = GameData.map[i,GameData.gridHeight - j - 1];
                newgrid.transform.SetParent(TilesParent.gameObject.transform);
                newgrid.GetComponent<GridTileManager>().mapGrid = mapGrid;
                newgrid.GetComponent<GridTileManager>().mapX = i;
                newgrid.GetComponent<GridTileManager>().mapY = GameData.gridHeight - j - 1;
                newgrid.GetComponent<GridTileManager>().gridType = mapGrid.type;
                newgrid.GetComponent<GridTileManager>().gameManagerObject = this.gameObject;
                newgrid.name = mapGrid.GridTypeToWord + " " +  i + " " + (GameData.gridHeight - j - 1);
                newgrid.transform.position = new Vector3((i*2)-5,(j*2)-1,0);
                newgrid.GetComponent<GridTileManager>().UpdateData();
            }
        }
    }

    public void FillGrid() {

    }
}
