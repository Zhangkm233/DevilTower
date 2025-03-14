using NUnit.Framework.Constraints;
using System;
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
    public void InitializingGrid() {
        GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
        foreach (GameObject grid in grids) {
            switch (grid.GetComponent<GridTileManager>().mapY) {
                case 12:
                    grid.GetComponent<SpriteRenderer>().sortingOrder = 97;
                    break;
                case 13:
                    grid.GetComponent<SpriteRenderer>().sortingOrder = 98;
                    break;
                case 14:
                    grid.GetComponent<SpriteRenderer>().sortingOrder = 99;
                    break;
            }
            grid.GetComponent<GridTileManager>().InitialData();
            grid.GetComponent<GridTileManager>().UpdateData();
        }
    }
    [Obsolete("Use InitializingGrid instead")]
    public void DrawThreeGrid() {
        for (int j = 0;j < 3;j++) {
            for (int i = 0;i < GameData.gridWidth;i++) {
                GameObject newgrid = Instantiate(gridPrefab);
                Grid mapGrid = GameData.map[i,GameData.gridHeight - j - 1];
                newgrid.layer = 6;
                switch (j) {
                    case 2:
                        newgrid.transform.localScale = new Vector3(0.8f,0.8f,1f);
                        break;
                    case 1:
                        newgrid.transform.localScale = new Vector3(0.9f,0.9f,1f);
                        break;
                    case 0:
                        newgrid.transform.localScale = new Vector3(1f,1f,1f);
                        break;
                }
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
}
