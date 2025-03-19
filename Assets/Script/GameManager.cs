using JetBrains.Annotations;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    //用于显示gamedata的变量
    [Header("GameData的数据")]
    public int gamelayer;
    public int gamekey1;
    public int gamekey2;
    public int gamekey3;
    public int gameplayerHp;
    public int gameplayerAtk;
    public int gameplayerDef;
    public int gameEventEncountered;
    public int gameMapWidth;
    public int gameMapHeight;
    public bool[] tarotUnlock;
    public bool[] tarotMissionUnlock; 
    [Space(15)]
    public Camera mainCamera;
    public GameObject objectClick;
    public GameObject catalogObject;
    public GridTileManager gridTileManager;
    public Grid GridInMap;

    void Update() {
        objectClick = ObjectClick();
        if (objectClick != null) {
            gridTileManager = objectClick.GetComponent<GridTileManager>();
            GridInMap = GameData.map[gridTileManager.mapX,gridTileManager.mapY];
        }
        if (Input.GetMouseButtonDown(0) && objectClick != null) {
            if (MapClickEvent()) {
                MonsterMovement();
                GameData.eventEncounter++;
            }
            //记录遭遇的事件数量
        }
        UpdateGameDataToPublic();
    }
    public void PlayerStatChange(int hp,int atk,int def,int key1,int key2,int key3,int gold,int forgetime) {
        GameData.playerHp = hp;
        GameData.playerAtk = atk;
        GameData.playerDef = def;
        GameData.key1 = key1;
        GameData.key2 = key2;
        GameData.key3 = key3;
        GameData.gold = gold;
        GameData.forgeTime = forgetime;
    }
    public void LayerChangeTo(int layerTo) {
        LayerChangeTo(layerTo,false);
    }
    public void LayerChangeTo(int layerTo, bool isInherit) {
        if(this.GetComponent<UIManager>().State != UIManager.UIState.STAT) return;
        GameData.layer = layerTo;
        GameData.eventEncounter = 0;
        if (!isInherit) {
            switch (layerTo) {
                case 1:
                    PlayerStatChange(400,10,10,0,0,0,0,0);
                    break;
                case 2:
                    PlayerStatChange(600,23,20,0,0,0,51,3);
                    break;
                case 3:
                    PlayerStatChange(800,43,46,0,0,0,134,6);
                    break;
            }
        }
        //重置UI和铁匠铺
        this.GetComponent<UIManager>().GoStat();
        this.GetComponent<UIManager>().goForgeButton.SetActive(false);
        //因为不同尺寸的地图，事件的位置会对不上格子，所以要重新给所有格子改名和更改mapY
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/map" + layerTo + ".txt");
        if(lines.Length != GameData.gridHeight) {
            GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
            foreach (GameObject grid in grids) {
                grid.GetComponent<GridTileManager>().mapY += lines.Length - GameData.gridHeight;
            }
        }
        //更新怪物数据到图鉴
        catalogObject.GetComponent<MonsterCatalogManager>().UpdateMonsterData();
        //加载地图到map里
        this.GetComponent<GridLoader>().LoadMapFromTxt(); 
        //刷新格子
        UpdateEachGrid();
    }
    bool MapClickEvent() {
        if (this.GetComponent<UIManager>().State != UIManager.UIState.STAT) {
            Debug.Log("不在正确的UISTATE里");
            return false;
        }
        if (gridTileManager.mapY != GameData.gridHeight - 1) {
            Debug.Log("不在最底层");
            return false;
        }
        if (gridTileManager.gridType == Grid.GridType.MONSTER) {
            //遭遇怪物
            if (((GridMonster)GridInMap).isBoss && !GameData.hasEncounterBoss) { 
                this.GetComponent<UIManager>().StartDialogBeforeBoss();
                GameData.hasEncounterBoss = true;
                return false;
            }
            //腐蚀
            GameData.playerHp = (int)(GameData.playerHp * ((GridMonster)GridInMap).CorruptionPercent());
            int battleDamage = ResolveDamage((GridMonster)GridInMap);
            if (battleDamage == -1) {
                Debug.Log("战斗伤害;???");
                return false;
            }
            Debug.Log("战斗伤害;" + battleDamage);
            GameData.playerHp -= battleDamage;
            GameData.gold += ((GridMonster)GridInMap).gold;
            if(((GridMonster)GridInMap).isBoss && GameData.hasEncounterBoss) {
                //如果是boss 生成传送门
                this.GetComponent<UIManager>().StartDialogAfterBoss();
                GameData.map[gridTileManager.mapX,gridTileManager.mapY] = new Grid("P",1);
                UpdateEachGrid();
                GameData.hasEncounterBoss = false;
                return true;
            }
            ClearGridInMap(gridTileManager);
            return true;
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
            return true;
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
            return true;
        }
        //捡血瓶
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            GameData.playerHp += ((GridBottle)gridTileManager.mapGrid).healingPoints;
            Debug.Log("捡到了血瓶，血量+" + ((GridBottle)gridTileManager.mapGrid).healingPoints);
            ClearGridInMap(gridTileManager);
            return true;
        }
        //开门
        if (gridTileManager.gridType == Grid.GridType.DOOR) {
            GridDoor door = ((GridDoor)gridTileManager.mapGrid);
            if (door.doorStat == GridDoor.DoorType.BRONZE) {
                if(GameData.key1 > 0) {
                    GameData.key1--;
                    Debug.Log("打开了" + door.doorStat + "门");
                    ClearGridInMap(gridTileManager);
                    return true;
                } else {
                    Debug.Log("青铜钥匙不足！");
                    return false;
                }
            }
            if (door.doorStat == GridDoor.DoorType.SILVER) {
                if (GameData.key2 > 0) {
                    GameData.key2--;
                    Debug.Log("打开了" + door.doorStat + "门");
                    ClearGridInMap(gridTileManager);
                    return true;
                } else {
                    Debug.Log("白银钥匙不足！");
                    return false;
                }
            }
            if (door.doorStat == GridDoor.DoorType.GOLD) {
                if (GameData.key3 > 0) {
                    GameData.key3--;
                    Debug.Log("打开了" + door.doorStat + "门");
                    ClearGridInMap(gridTileManager);
                    return true;
                } else {
                    Debug.Log("黄金钥匙不足！");
                    return false;
                }
            }
        }
        if (gridTileManager.gridType == Grid.GridType.NPC) {
            if (GameData.layer != 1) {
                ClearGridInMap(gridTileManager);
                return true;
            }
            int dialogStat = GridInMap.stat;
            this.GetComponent<UIManager>().StartDialog(dialogStat);
            ClearGridInMap(gridTileManager);
            return true;
        }
        if (gridTileManager.gridType == Grid.GridType.SHOP) {
            this.GetComponent<UIManager>().StartTrade((GridShop)GridInMap,
                gridTileManager.mapX,gridTileManager.mapY);
            return false;
        }
        if (gridTileManager.gridType == Grid.GridType.EVENT) {
            if(((GridEvent)GridInMap).eventType == GridEvent.EventType.CORRUPTIONROOTSAVE) {
                ((GridEvent)GridInMap).hpSave = (int)(GameData.playerHp * 0.5);
                GameData.playerHp -= ((GridEvent)GridInMap).hpSave;
                ((GridEvent)GridInMap).eventType = GridEvent.EventType.CORRUPTIONROOTLOAD;
                return false;
            }
            if (((GridEvent)GridInMap).eventType == GridEvent.EventType.CORRUPTIONROOTLOAD) {
                GameData.playerHp += ((GridEvent)GridInMap).hpSave;
                ClearGridInMap(gridTileManager);
                return true;
            }
            this.GetComponent<UIManager>().StartEvent((GridEvent)GridInMap,
                gridTileManager.mapX,gridTileManager.mapY);
            return false;
        }
        if (gridTileManager.gridType == Grid.GridType.PORTAL) {
            //进入塔罗牌
            LayerChangeTo(GameData.layer + 1,true);
            this.GetComponent<UIManager>().GoTarot();
            return false;
        }
        return false;
    }
    
    public void MonsterMovement() {
        bool hasLostMind = false;
        //处理最地下一层怪物的移动逻辑
        for (int i = 0;i < GameData.gridWidth;i++) {
            //魔心的判断 防止让他一步走两格
            if (hasLostMind && i == 5) continue;

            Grid grid = GameData.map[i,GameData.gridHeight - 1];
            if (grid.type == Grid.GridType.MONSTER) {
                if (((GridMonster)grid).isStalk) {
                    if (((GridMonster)grid).stalkTurn == 2) {
                        //追猎
                        Debug.Log(((GridMonster)grid).name + "追猎触发");
                        GameData.playerHp -= ((GridMonster)grid).atk - GameData.playerDef;
                        ((GridMonster)grid).stalkTurn = 0;
                    } else {
                        ((GridMonster)grid).stalkTurn++;
                    }
                }
                //坚定
                if (((GridMonster)grid).isFirmness) continue;
                //魔心
                if (i == 0 && !((GridMonster)grid).isLostmind) continue;
                //如果在第一个格子且没有魔心，那么不移动
                Grid targetGrid = null;
                //如果有魔心且最右边不是怪，那么尝试移动到最右边
                if (i == 0 && ((GridMonster)grid).isLostmind && 
                    (GameData.map[5,GameData.gridHeight - 1]).type != Grid.GridType.MONSTER) {
                    hasLostMind = true;
                    targetGrid = GameData.map[5,GameData.gridHeight - 1];
                }
                if (i == 0 && ((GridMonster)grid).isLostmind &&
                    (GameData.map[5,GameData.gridHeight - 1]).type == Grid.GridType.MONSTER) {
                    continue;
                }
                if (targetGrid == null) targetGrid = GameData.map[i - 1,GameData.gridHeight - 1];
                if (targetGrid.type != Grid.GridType.MONSTER &&
                     targetGrid.type != Grid.GridType.DOOR &&
                     targetGrid.type != Grid.GridType.BARRIER &&
                     targetGrid.type != Grid.GridType.PORTAL) {
                    //与左边的格子交换
                    if (i == 0 && ((GridMonster)grid).isLostmind) {
                        GameData.map[5,GameData.gridHeight - 1] = grid;
                    } else {
                        GameData.map[i - 1,GameData.gridHeight - 1] = grid;
                    }
                    GameData.map[i,GameData.gridHeight - 1] = targetGrid;
                    if (((GridMonster)grid).isCrack && targetGrid.type != Grid.GridType.SHOP && targetGrid.type != Grid.GridType.NPC) {
                        //碎裂
                        ClearGridInMap(i,GameData.gridHeight - 1);
                    }
                }
            }
        }
        UpdateEachGrid();
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
        gameEventEncountered = GameData.eventEncounter;
        gameMapWidth = GameData.gridWidth;
        gameMapHeight = GameData.gridHeight;
        tarotUnlock = GameData.tarotUnlock;
        tarotMissionUnlock = GameData.tarotMissionUnlock;
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
