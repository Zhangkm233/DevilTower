using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public Sprite gridSprite;
    void Start() {
        DrawThreeGrid();
    }

    public void DrawThreeGrid() {
        for (int i = 0;i < GameData.gridWidth;i++) {
            for (int j = 0;j < 3;j++) {
                GameObject grid = new GameObject("Grid" + i + j);
                grid.transform.position = new Vector3((i*2)-5,(j*2)-1,0);
                grid.AddComponent<SpriteRenderer>();
                grid.GetComponent<SpriteRenderer>().sprite = gridSprite;
            }
        }
    }
}
