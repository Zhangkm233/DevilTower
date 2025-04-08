using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Grid;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT文件路径
    private int eventCount = 0;
    //public int gridWidth = GameData.gridWidth; // 地图宽度
    //public int gridHeight = GameData.gridHeight; // 地图高度
    public TextAsset[] maps;
    void Awake() {

    }
    public void InitialzeMapAndGrid() {
        LoadMapFromTxt();
        PrintGrid();
        InitializingGrid();
    }
    public void ChangeSaveSlot(TMP_Dropdown dd) {
        GameData.saveSlotChoose = dd.value;
    }
    public void SaveAll() {
        if(GameData.saveSlotChoose == 0) {
            // 不能覆盖自动存档
            Debug.Log("不能覆盖自动存档");
            return;
        }
        SaveManager.Save(GameData.saveSlotChoose);
    }
    public void LoadAll() {
        SaveManager.Load(GameData.saveSlotChoose);
        this.GetComponent<GameManager>().UpdateEachGrid();
    }
    public void DeleteAll() {
        if (GameData.saveSlotChoose == 0) {
            // 不能删除自动存档
            Debug.Log("不能删除自动存档");
            return;
        }
        SaveManager.Delete(GameData.saveSlotChoose);
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
        Debug.Log("更改GridHeight为" + lines.Length);
        eventCount = 0;
        GameData.map = new Grid[GameData.gridWidth,GameData.gridHeight];
        for (int y = 0;y < GameData.gridHeight;y++) {
            string[] tiles = lines[y].Split(','); 
            for (int x = 0;x < GameData.gridWidth;x++) {
                string gridType = tiles[x].Split(' ')[0];
                int gridStat = int.Parse(tiles[x].Split(' ')[1]);
                print("x:" + x + " y:" + y + " gridType:" + gridType + " gridStat:" + gridStat);
                if (gridType != "X") eventCount++;
                //写进map里
                writeInGrid(x,y,gridType,gridStat);
            }
        }
        GameData.eventCount = eventCount;
    }

    void PrintGrid() {
        for (int y = 0;y < GameData.gridHeight;y++) {
            for (int x = 0;x < GameData.gridWidth;x++) {
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
        g.x = x;
        g.y = y;
        g.fromX = x;
        g.fromY = y;
    }
}
