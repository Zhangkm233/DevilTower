using JetBrains.Annotations;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //用于显示gamedata的变量
    public int gamelayer;
    public int gamekey1;
    public int gamekey2;
    public int gamekey3;
    public int gameplayerHp;
    public int gameplayerAtk;
    public int gameplayerDef;

    public Camera mainCamera;
    public GameObject objectClick;
    public GridTileManager gridTileManager;
    public Grid GridInMap;

    void Update() {
        objectClick = ObjectClick();
        if (objectClick != null) {
            gridTileManager = objectClick.GetComponent<GridTileManager>();
            GridInMap = GameData.map[gridTileManager.mapX,gridTileManager.mapY];
        }
        if (Input.GetMouseButtonDown(0) && objectClick != null) {
            MapClickEvent();
            MonsterMovement();
        }
        UpdateGameDataToPublic();
    }

    void MapClickEvent() {
        if (gridTileManager.mapY != GameData.gridHeight-1) return;
        //遭遇怪物
        if (gridTileManager.gridType == Grid.GridType.MONSTER) {
            int battleDamage = ResolveDamage((GridMonster)GridInMap);
            if (battleDamage == -1) {
                Debug.Log("战斗伤害：???");
                return;
            }
            Debug.Log("战斗伤害：" + battleDamage);
            GameData.playerHp -= battleDamage;
            ClearGridInMap(gridTileManager);
            return;
        }
        //捡钥匙
        if (gridTileManager.gridType == Grid.GridType.KEY) {
            switch (gridTileManager.mapGrid.stat) {
                case 1: 
                    GameData.key1++;
                    Debug.Log("捡到了青铜钥匙"); 
                    break;
                case 2: 
                    GameData.key2++;
                    Debug.Log("捡到了白银钥匙");
                    break;
                case 3: 
                    GameData.key3++;
                    Debug.Log("捡到了黄金钥匙"); 
                    break;
            }
            ClearGridInMap(gridTileManager);
            return;
        }
        //捡宝石
        if (gridTileManager.gridType == Grid.GridType.GEM) {
            if(((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.ATK) {
                GameData.playerAtk += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("捡到了攻击宝石，攻击力+" + ((GridGem)gridTileManager.mapGrid).AddSum);
            }
            if (((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.DEF) {
                GameData.playerDef += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("捡到了防御宝石，防御力+" + ((GridGem)gridTileManager.mapGrid).AddSum);
            }
            ClearGridInMap(gridTileManager);
            return;
        }
        //捡血瓶
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            GameData.playerHp += ((GridBottle)gridTileManager.mapGrid).healingPoints;
            Debug.Log("捡到了血瓶，血量+" + ((GridBottle)gridTileManager.mapGrid).healingPoints);
            ClearGridInMap(gridTileManager);

            //有bug，过会改
            return;
        }
        //开门
        if (gridTileManager.gridType == Grid.GridType.DOOR) {
            GridDoor door = ((GridDoor)gridTileManager.mapGrid);
            if (door.doorStat == GridDoor.DoorType.BRONZE) {
                if(GameData.key1 > 0) {
                    GameData.key1--;
                    Debug.Log("打开了" + door.doorStat + "门");
                    ClearGridInMap(gridTileManager);
                } else {
                    Debug.Log("青铜钥匙不足！");
                }
            }
            if (door.doorStat == GridDoor.DoorType.SILVER) {
                if (GameData.key2 > 0) {
                    GameData.key2--;
                    Debug.Log("打开了" + door.doorStat + "门");
                    ClearGridInMap(gridTileManager);
                } else {
                    Debug.Log("白银钥匙不足！");
                }
            }
            if (door.doorStat == GridDoor.DoorType.GOLD) {
                if (GameData.key3 > 0) {
                    GameData.key3--;
                    Debug.Log("打开了" + door.doorStat + "门");
                    ClearGridInMap(gridTileManager);
                } else {
                    Debug.Log("黄金钥匙不足！");
                }
            }
        }
        if (gridTileManager.gridType == Grid.GridType.NPC) {
            ClearGridInMap(gridTileManager);
        }
        if (gridTileManager.gridType == Grid.GridType.SHOP) {
            ClearGridInMap(gridTileManager);
        }
    }

    public void MonsterMovement() {
        bool hasLostMind = false;
        //处理最地下一层怪物的移动逻辑
        for (int i = 0;i < GameData.gridWidth;i++) { 
            if (hasLostMind && i == 5) continue;
            Debug.Log("判断" + i + " " + (GameData.gridHeight - 1));
            Grid grid = GameData.map[i,GameData.gridHeight - 1];
            if (grid.type == Grid.GridType.MONSTER) {
                if (((GridMonster)grid).isFirmness) continue;
                if (i == 0 && !((GridMonster)grid).isLostmind) continue;
                Grid targetGrid = null;
                if (i == 0 && ((GridMonster)grid).isLostmind) {
                    hasLostMind = true;
                    targetGrid = GameData.map[5,GameData.gridHeight - 1];
                }
                if(targetGrid == null) targetGrid = GameData.map[i - 1,GameData.gridHeight - 1];

                if (targetGrid.type != Grid.GridType.MONSTER &&
                     targetGrid.type != Grid.GridType.DOOR) {
                    //与左边的格子交换
                    if (i == 0 && ((GridMonster)grid).isLostmind) {
                        GameData.map[5,GameData.gridHeight - 1] = grid;
                    } else {
                        GameData.map[i - 1,GameData.gridHeight - 1] = grid;
                    }
                    GameData.map[i,GameData.gridHeight - 1] = targetGrid;
                    if (((GridMonster)grid).isCrack) {
                        ClearGridInMap(i,GameData.gridHeight - 1);
                    }
                    UpdateEachGrid();
                }
            }
        }
    }
        
    public void ClearGridInMap(GridTileManager gridTileManager) {
        ClearGridInMap(gridTileManager.mapX,gridTileManager.mapY);
    }
    public void ClearGridInMap(int mapX,int mapY) {
        int i;
        for (i = mapY; i > 0; i--) {
            if (GameData.map[mapX,i - 1] == null) break;
            GameData.map[mapX,i] = GameData.map[mapX,i-1];
        }
        GameData.map[mapX,i] = null;
        UpdateEachGrid();
    }
    public void UpdateEachGrid() {
        GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
        foreach (GameObject grid in grids) {
            grid.GetComponent<GridTileManager>().UpdateData();
        }
    }
    public int ResolveDamage(GridMonster monster) {
        //结算时用的伤害 给防杀和攻杀提供地方写
        if (GameData.playerAtk - (monster.def + monster.hp) >= 0) {
            Debug.Log("攻杀");
            //如果攻击力大于怪物防御力 + 生命值，那么是攻杀
            if (GameData.playerDef > monster.atk) {
                Debug.Log("防杀");
                return 0;//如果同时玩家防御力大于怪物攻击力，那么也是防杀
            }
            return 0;
        }
        if (GameData.playerDef >= monster.atk && GameData.playerAtk > monster.def) {
            Debug.Log("防杀");
            return 0;//如果防御大于怪物攻击力，且玩家攻击力大于怪物防御力，那么是防杀
        }
        return CaculateDamage(monster);
    }

    public int CaculateDamage(GridMonster monster) {
        if(GameData.playerAtk - (monster.def + monster.hp) >= 0 ) {
            if (GameData.playerDef > monster.atk) {
                return 0;
            }
            return 0;
        }
        if(GameData.playerDef >= monster.atk && GameData.playerAtk > monster.def) {
            return 0;
        }
        if (GameData.playerAtk - monster.def <= 0) {
            return -1;//如果玩家攻击力小于怪物防御力，那么显示???
        }
        if(GameData.playerAtk - monster.def > 0) {
            return (monster.hp / (GameData.playerAtk - monster.def)) * (monster.atk - GameData.playerDef);
        }//正常显示伤害
        return -2;//errrrror
    }
    public void UpdateGameDataToPublic() {
        gamelayer = GameData.layer;
        gamekey1 = GameData.key1;
        gamekey2 = GameData.key2;
        gamekey3 = GameData.key3;
        gameplayerAtk = GameData.playerAtk;
        gameplayerDef = GameData.playerDef;
        gameplayerHp = GameData.playerHp;
    }

    public GameObject ObjectClick() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit hit)) {
            GameObject clickedObject = hit.collider.gameObject;
            return clickedObject;
        }
        return null;
    }
}
