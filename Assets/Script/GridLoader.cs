using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Grid;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT文件路径
    public int gridWidth = GameData.gridWidth; // 地图宽度
    public int gridHeight = GameData.gridHeight; // 地图高度
    public TextAsset[] maps;
    void Awake() {
        LoadMapFromTxt();
        PrintGrid();
        InitializingGrid();
    }
    public void SaveAll() {
        SaveManager.Save();
    }
    public void LoadAll() {
        SaveManager.Load();
        this.GetComponent<GameManager>().UpdateEachGrid();
    }

    public void InitializingGrid() {
        // 初始化所有格子
        GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
        foreach (GameObject grid in grids) {
            grid.GetComponent<GridTileManager>().InitialData();
            grid.GetComponent<GridTileManager>().UpdateData();
        }
    }
    //M怪物 X封锁 D门 G宝石 B血瓶 K钥匙 N NPC S商人

    public void LoadMapFromTxt() {
        // 把TXT文件的地图存储到GameData.map里
        txtFilePath = Application.streamingAssetsPath + "/map" + GameData.layer + ".txt";
        // 读取TXT文件
        string[] lines = File.ReadAllLines(txtFilePath);
        GameData.gridHeight = lines.Length;
        GameData.map = new Grid[GameData.gridWidth,GameData.gridHeight];
        for (int y = 0;y < GameData.gridHeight;y++) {
            string[] tiles = lines[y].Split(','); 
            for (int x = 0;x < gridWidth;x++) {
                string gridType = tiles[x].Split(' ')[0];
                int gridStat = int.Parse(tiles[x].Split(' ')[1]);
                print("x:" + x + " y:" + y + " gridType:" + gridType + " gridStat:" + gridStat);
                //写进map里
                writeInGrid(x,y,gridType,gridStat);
            }
        }
    }

    void PrintGrid() {
        for (int y = 0;y < GameData.gridHeight;y++) {
            for (int x = 0;x < gridWidth;x++) {
                print("x:" + x + " y:" + y + " gridType:" + GameData.map[x,y].type + " gridStat:" + GameData.map[x,y].stat);
                if (GameData.map[x,y].type == GridType.MONSTER) {
                    GridMonster gm = (GridMonster)GameData.map[x,y];
                    print("name:" + gm.name + "atk:" + gm.atk + " def:" + gm.def + " hp:" + gm.hp + " gold:" + gm.gold);
                    if (gm.isLostmind) print("魔心");
                    if (gm.isCrack) print("破碎");
                    if (gm.isFirmness) print("坚定");
                    if (gm.isStalk) print("追猎");
                    if (gm.isCorruptionOne) print("腐坏1");
                    if (gm.isCorruptionTwo) print("腐坏2");
                    if (gm.isCorruptionThree) print("腐坏3");
                    if (gm.isBoss) print("头目");
                }
            }
        }
    }

    public void writeInGrid(int x,int y,string gridType,int gridStat) {
        Grid g;
        switch (gridType) {
            case "M":
                g = new GridMonster(gridStat);
                break;
            case "G":
                g = new GridGem(gridStat);
                break;
            case "D":
                g = new GridDoor(gridStat);
                break;
            case "K":
                g = new GridKey(gridStat);
                break;
            case "B":
                g = new GridBottle(gridStat);
                break;
            case "S":
                g = new GridShop(gridStat);
                break;
            case "E":
                g = new GridEvent(gridStat);
                break;
            default:
                g = new Grid(gridType,gridStat);
                break;
        }
        g.GridTypeToWord = gridType;
        GameData.map[x,y] = g;
    }
}
