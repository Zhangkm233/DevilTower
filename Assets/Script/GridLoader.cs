using System.IO;
using UnityEngine;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT�ļ�·��
    public int gridWidth = GameData.gridWidth; // ��ͼ���
    public int gridHeight = GameData.gridHeight; // ��ͼ�߶�
    void Start() {
        LoadMapFromTxt();
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

    void writeInGrid(int x,int y,string gridType,int gridStat) {
        GameData.map[x,y] = new Grid(gridType,gridStat);
    }
}
