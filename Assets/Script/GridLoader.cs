using System.IO;
using UnityEngine;
using static Grid;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT�ļ�·��
    public int gridWidth = GameData.gridWidth; // ��ͼ���
    public int gridHeight = GameData.gridHeight; // ��ͼ�߶�
    void Start() {
        LoadMapFromTxt();
        PrintGrid();
        this.gameObject.GetComponent<GridDrawer>().DrawThreeGrid();
    }
    //M���� X���� D�� G��ʯ BѪƿ KԿ�� N NPC S����
    void LoadMapFromTxt() { 
        txtFilePath = "Assets/Resources/map" + GameData.layer + ".txt"; // ��ȡTXT�ļ�
        // ��ȡTXT�ļ�
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

    void PrintGrid() {
        for (int y = 0;y < gridHeight;y++) {
            for (int x = 0;x < gridWidth;x++) {
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

    void writeInGrid(int x,int y,string gridType,int gridStat) {
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
            default:
                g = new Grid(gridType,gridStat);
                break;
        }
        g.GridTypeToWord = gridType;
        GameData.map[x,y] = g;
    }
}
