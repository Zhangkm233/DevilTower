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
                GameObject grid = Instantiate(gridPrefab);
                grid.name = "Grid" + i + j;
                grid.transform.position = new Vector3((i*2)-5,(j*2)-1,0);
                TMP_Text gridtext = grid.GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>();
                gridtext.text = GameData.map[i,GameData.gridHeight-j-1].GridTypeToWord;
            }
        }
    }

    public void FillGrid() {

    }
}
