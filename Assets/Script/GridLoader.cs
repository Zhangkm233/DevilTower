using System.IO;
using UnityEngine;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public string txtFilePath ; // TXT文件路径
    public int gridWidth = GameData.gridWidth; // 地图宽度
    public int gridHeight = GameData.gridHeight; // 地图高度
    void Start() {
        LoadMapFromTxt();
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

    void writeInGrid(int x,int y,string gridType,int gridStat) {
        GameData.map[x,y] = new Grid(gridType,gridStat);
    }
}
