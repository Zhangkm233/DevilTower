using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Grid;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT�ļ�·��
    private int eventCount = 0;
    //public int gridWidth = GameData.gridWidth; // ��ͼ���
    //public int gridHeight = GameData.gridHeight; // ��ͼ�߶�
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
            // ���ܸ����Զ��浵
            Debug.Log("���ܸ����Զ��浵");
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
            // ����ɾ���Զ��浵
            Debug.Log("����ɾ���Զ��浵");
            return;
        }
        SaveManager.Delete(GameData.saveSlotChoose);
    }

    public void InitializingGrid() {
        // ��ʼ�����и���
        GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
        foreach (GameObject grid in grids) {
            grid.GetComponent<GridTileManager>().InitialData();
            grid.GetComponent<GridTileManager>().UpdateData();
        }
    }
    //M���� X���� D�� G��ʯ BѪƿ KԿ�� N NPC S����

    public void LoadMapFromTxt() {
        // ��TXT�ļ��ĵ�ͼ�洢��GameData.map��
        txtFilePath = Application.streamingAssetsPath + "/map" + GameData.layer + ".txt";
        // ��ȡTXT�ļ�
        string[] lines = File.ReadAllLines(txtFilePath);
        GameData.gridHeight = lines.Length;
        Debug.Log("����GridHeightΪ" + lines.Length);
        eventCount = 0;
        GameData.map = new Grid[GameData.gridWidth,GameData.gridHeight];
        for (int y = 0;y < GameData.gridHeight;y++) {
            string[] tiles = lines[y].Split(','); 
            for (int x = 0;x < GameData.gridWidth;x++) {
                string gridType = tiles[x].Split(' ')[0];
                int gridStat = int.Parse(tiles[x].Split(' ')[1]);
                print("x:" + x + " y:" + y + " gridType:" + gridType + " gridStat:" + gridStat);
                if (gridType != "X") eventCount++;
                //д��map��
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
                    if (gm.isLostmind) print("ħ��");
                    if (gm.isCrack) print("����");
                    if (gm.isFirmness) print("�ᶨ");
                    if (gm.isStalk) print("׷��");
                    if (gm.isCorruptionOne) print("����1");
                    if (gm.isCorruptionTwo) print("����2");
                    if (gm.isCorruptionThree) print("����3");
                    if (gm.isBoss) print("ͷĿ");
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
