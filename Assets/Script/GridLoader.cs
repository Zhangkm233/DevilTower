using System.IO;
using UnityEngine;
using static Grid;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT文件路径
    public int gridWidth = GameData.gridWidth; // 地图宽度
    public int gridHeight = GameData.gridHeight; // 地图高度
    void Start() {
        LoadMapFromTxt();
        printGrid();
        this.gameObject.GetComponent<GridDrawer>().DrawThreeGrid();
    }
    //M怪物 X封锁 D门 G宝石 B血瓶 K钥匙 N NPC S商人
    void LoadMapFromTxt() { 
        txtFilePath = "Assets/Resources/map" + GameData.layer + ".txt"; // 读取TXT文件
        // 读取TXT文件
        string[] lines = File.ReadAllLines(txtFilePath);
        for (int y = 0;y < gridHeight;y++) {
            string[] tiles = lines[y].Split(','); 
            for (int x = 0;x < gridWidth;x++) {
                string gridType = tiles[x].Split(' ')[0];
                int gridStat = int.Parse(tiles[x].Split(' ')[1]);
                print("x:" + x + " y:" + y + " gridType:" + gridType + " gridStat:" + gridStat);
                writeInGrid(x,y,gridType,gridStat);
            }
        }
    }

    void printGrid() {
        for (int y = 0;y < gridHeight;y++) {
            for (int x = 0;x < gridWidth;x++) {
                print("x:" + x + " y:" + y + " gridType:" + GameData.map[x,y].type + " gridStat:" + GameData.map[x,y].stat);
                if (GameData.map[x,y].type == GridType.MONSTER) {
                    GridMonster gm = (GridMonster)GameData.map[x,y];
                    print("name:" + gm.name + "atk:" + gm.atk + " def:" + gm.def + " hp:" + gm.hp + " gold:" + gm.gold);
                }
            }
        }
    }

    void writeInGrid(int x,int y,string gridType,int gridStat) {
        Grid g;
        switch (gridType) {
            case "M":
                g = new GridMonster(gridStat);
                break;
            default:
                g = new Grid(gridType,gridStat);
                break;
        }
        GameData.map[x,y] = g;
    }
}
