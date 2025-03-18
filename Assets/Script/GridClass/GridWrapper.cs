using UnityEngine;
using static Grid;

[System.Serializable]
public class GridWrapper
{
    [SerializeField]
    //public Grid[,] grid = new Grid[GameData.gridWidth,GameData.gridHeight];
    public Grid[] grid;

    public GridWrapper(int width,int height) {
        grid = new Grid[width * height];
    }

    // 将二维数组转换为一维数组
    public void SetGrid(Grid[,] sourceGrid,int width,int height) {
        for (int i = 0;i < width;i++) {
            for (int j = 0;j < height;j++) {
                grid[i * height + j] = sourceGrid[i,j];
            }
        }
    }

    // 将一维数组转换为二维数组
    public Grid[,] GetGrid(int width,int height) {
        Grid[,] resultGrid = new Grid[width,height];
        for (int i = 0;i < width;i++) {
            for (int j = 0;j < height;j++) {
                Grid g;
                int gridStat = grid[i * height + j].stat;
                string gridType = grid[i * height + j].GridTypeToWord;
                switch (grid[i * height + j].GridTypeToWord) {
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
                resultGrid[i,j] = g;
            }
        }
        return resultGrid;
    }
}
