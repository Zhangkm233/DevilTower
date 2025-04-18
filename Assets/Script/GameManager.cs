
using UnityEngine;
using static Grid;


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
    public string lastMonsterName;
    public int[] tarotEquip;
    public bool isDeathBuff;
    public int forgeTime;
    public int atkOffsetInt;
    public int defOffsetInt;

    [Space(15)]
    public Camera mainCamera;
    public GameObject objectClick;
    public GameObject catalogObject;
    public GridTileManager gridTileManager;
    public Grid GridInMap;
    public GameObject audioManagerObject;
    public AudioManager audioManagerScript;

    void Update() {
        objectClick = ObjectClick();
        if (objectClick != null && this.GetComponent<UIManager>().State != UIManager.UIState.STANDBY) {
            gridTileManager = objectClick.GetComponent<GridTileManager>();
            GridInMap = GameData.map[gridTileManager.mapX,gridTileManager.mapY];
        }
        if (Input.GetMouseButtonDown(0) && objectClick != null) {
            if (MapClickEvent()) {
                MonsterMovement();
                //GameData.allGame_EventEncountered++;
                GameData.eventEncounter++;
            }
            //记录遭遇的事件数量
        }
        UpdatePlayerOffset();
        UpdateGameDataToPublic();
    }
    private void Start() {
        StartGame();
    }
    public void StartGame() {
        this.GetComponent<GridLoader>().InitialzeMapAndGrid();
        this.GetComponent<UIManager>().InitializeUI();
        SaveManager.LoadForeverData();
        audioManagerObject.GetComponent<AudioManager>().PlayBgm(GameData.layer - 1);
        audioManagerObject.GetComponent<AudioManager>().UpdateVolume();
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
        this.GetComponent<UIManager>().GoState(UIManager.UIState.STANDBY);
        GameData.layer = layerTo;
        GameData.eventEncounter = 0;
        /*
        GameData.defeatSHWQJ = 0;
        GameData.defeatSHDPJ = 0;
        GameData.isEventUsed = false;*/
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
        if (layerTo <= 2) {
            this.GetComponent<UIManager>().goForgeButton.SetActive(false);
        } else {
            this.GetComponent<UIManager>().goForgeButton.SetActive(true);
        }
        //因为不同尺寸的地图，事件的位置会对不上格子，所以要重新给所有格子改名和更改mapY
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/map" + layerTo + ".txt");
        if(lines.Length != GameData.gridHeight) {
            GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
            foreach (GameObject grid in grids) {
                grid.GetComponent<GridTileManager>().mapY += lines.Length - GameData.gridHeight;
            }
        }
        //更新怪物数据到图鉴
        //catalogObject.GetComponent<MonsterCatalogManager>().UpdateMonsterData();
        //加载地图到map里
        this.GetComponent<GridLoader>().LoadMapFromTxt(); 
        //刷新格子
        UpdateEachGrid();
        //更新自动存档
        SaveManager.Save(0);
        //this.GetComponent<UIManager>().GoStat();
    }
    bool MapClickEvent() {
        audioManagerScript = audioManagerObject.GetComponent<AudioManager>();
        
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
            TarotManager tarots = this.GetComponent<TarotManager>();
            /*
            if (((GridMonster)GridInMap).isBoss && !GameData.hasEncounterBoss) {
                this.GetComponent<UIManager>().StartDialogBeforeBoss();
                //if((GameData.layer == 2) && (tarots.IsMissionUnlock("Strength"))) {
                //    tarots.UnlockTarot("Strength");
                //}
                GameData.hasEncounterBoss = true;
                return false;
            }*/
            //腐蚀
            GameData.playerHp = (int)(GameData.playerHp * ((GridMonster)GridInMap).CorruptionPercent());
            int battleDamage = ResolveDamage((GridMonster)GridInMap);
            if (battleDamage == -1) {
                Debug.Log("战斗伤害;???");
                return false;
            }
            Debug.Log("战斗伤害;" + battleDamage);
            audioManagerScript.PlayBattle();
            GetComponent<UIManager>().PopNumber(battleDamage, new Color(255.0f/255f,102.0f/255,28.0f/255));
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Devil"))) {
                Debug.Log("恶魔触发，获得等同于伤害的生命值");
                //恶魔
                //与怪物战斗会改为使你获得等同于伤害的生命值，但是血瓶会改为对你造成等同于回复量双倍的伤害
                GameData.playerHp += battleDamage;
            } else {    
                GameData.playerHp -= battleDamage;
            }
            //全局金币
            //在所有游戏流程中，消灭5个恶魔（包括初等恶魔，中等恶魔，高等恶魔）
            /*
            if (((GridMonster)GridInMap).name == "初等恶魔" ||
                ((GridMonster)GridInMap).name == "中等恶魔" ||
                ((GridMonster)GridInMap).name == "高等恶魔") {
                GameData.allGame_DevilsDefeated++;
            }
            //杀死15个“燃尽之魂”“被噬之魂”或者“拘魂囚徒”
            if (((GridMonster)GridInMap).name == "燃尽之魂" ||
                ((GridMonster)GridInMap).name == "被噬之魂" ||
                ((GridMonster)GridInMap).name == "拘魂囚徒") {
                GameData.allGame_SoulDefeated++;
            }
            if (((GridMonster)GridInMap).name == "噬魂盾牌匠") {
                GameData.defeatSHDPJ++;
                if (GameData.defeatSHDPJ == 3 && GameData.isEventUsed == false) {
                    if (tarots.IsMissionUnlock("Pope")) tarots.UnlockTarot("Pope");
                }
            }
            if (((GridMonster)GridInMap).name == "噬魂武器匠") {
                GameData.defeatSHWQJ++;
                if (GameData.defeatSHWQJ == 3 && GameData.isEventUsed == false) {
                    if (tarots.IsMissionUnlock("Empress")) tarots.UnlockTarot("Empress");
                }
            }

            if (((GridMonster)GridInMap).name == GameData.lastMonsterName) {
                GameData.continueDefeatStatrack++;
                if((GameData.continueDefeatStatrack >= 4) && tarots.IsMissionUnlock("Tower")) {
                    tarots.UnlockTarot("Tower");
                }
            } else {
                GameData.continueDefeatStatrack = 1;
            }
            GameData.allGame_GoldGained += ((GridMonster)GridInMap).gold;
            */
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Judgment"))) {
                Debug.Log("审判触发，获得等同于锻造数值的攻击力和防御力");
                //审判
                //连续击败同一个怪物会使得你获得等同于锻造数值的攻击力和防御力
                if (((GridMonster)GridInMap).name == GameData.lastMonsterName) {
                    GameData.playerAtk += GameData.forgeTime;
                    GameData.playerDef += GameData.forgeTime;
                    GameData.lastMonsterName = ((GridMonster)GridInMap).name;
                    //触发效果
                }
            }
            //重置死神的buff
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Death"))) {
                if (GameData.isDeathBuff == false) GameData.isDeathBuff = true;
            }
            GameData.gold += ((GridMonster)GridInMap).gold;
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
            audioManagerScript.PlayPick();
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
            audioManagerScript.PlayPick();
            ClearGridInMap(gridTileManager);
            return true;
        }
        //捡血瓶
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            int healingPoints = ((GridBottle)gridTileManager.mapGrid).healingPoints;
            //星星
            //血瓶额外提供25点生命值
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Star"))) {
                Debug.Log("星星触发，血瓶额外提供25点生命值");
                healingPoints += 25;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Devil"))) {
                Debug.Log("恶魔触发，血瓶对你造成等同于回复量双倍的伤害");
                //恶魔
                //血瓶对你造成等同于回复量双倍的伤害
                GameData.playerHp -= healingPoints * 2;
            } else {
                GameData.playerHp += healingPoints;
            }
            Debug.Log("捡到了血瓶，血量+" + healingPoints);
            GetComponent<UIManager>().PopNumber(healingPoints, Color.green);
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
                    //GameData.allGame_Door1Opened++;
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
            //隐者
            //在你完成NPC事件后，获得一把钥匙1

            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Hermit"))) {
                Debug.Log("隐者触发，获得一把钥匙1");
                GameData.key1++;
            }
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
            if (((GridEvent)GridInMap).eventType == GridEvent.EventType.BOOM) {
                if(gridTileManager.mapX != 0) ClearGridInMap(gridTileManager.mapX - 1,gridTileManager.mapY);
                if(gridTileManager.mapX != 5) ClearGridInMap(gridTileManager.mapX + 1,gridTileManager.mapY);
                ClearGridInMap(gridTileManager);
                return true;
            }
            this.GetComponent<UIManager>().StartEvent((GridEvent)GridInMap,
                gridTileManager.mapX,gridTileManager.mapY);
            return false;
        }
        if (gridTileManager.gridType == Grid.GridType.PORTAL) {
            //进入塔罗牌
            /*
            TarotManager tarots = this.GetComponent<TarotManager>();
            switch (GameData.layer) {
                case 1:
                    if (tarots.IsMissionUnlock("Chariot")) tarots.UnlockTarot("Chariot");
                    if (GameData.key1 + GameData.key2 + GameData.key3 >= 5 && tarots.IsMissionUnlock("Magician")) {
                        tarots.UnlockTarot("Magician");
                    }
                    if (GameData.playerAtk >= 23 && tarots.IsMissionUnlock("Devil")) {
                        tarots.UnlockTarot("Devil");
                    }
                    break;
                case 2:
                    int monsterCount = 0;
                    //遍历map，查找是否没有怪物了
                    for (int y = 0;y < GameData.gridHeight;y++) {
                        for (int x = 0;x < GameData.gridWidth;x++) {
                            if (GameData.map[x,y] != null) {
                                if (GameData.map[x,y].type == GridType.MONSTER) monsterCount++;
                            }

                        }
                    }
                    if ((monsterCount == 0) && (tarots.IsMissionUnlock("Moon"))){
                        tarots.UnlockTarot("Moon");
                    }
                    break;
            }
            if (GameData.allGame_EventEncountered >= 1000 && tarots.IsMissionUnlock("WheelOfFortune")) {
                tarots.UnlockTarot("WheelOfFortune");
            }
            if (GameData.allGame_GoldGained >= 5000 && tarots.IsMissionUnlock("Emperor")) {
                tarots.UnlockTarot("Emperor");
            }
            if (GameData.allGame_SoulDefeated >= 15 && tarots.IsMissionUnlock("Judgment")) {
                tarots.UnlockTarot("Judgment");
            }
            if (GameData.allGame_DevilsDefeated >= 5 && tarots.IsMissionUnlock("Star")) {
                tarots.UnlockTarot("Star");
            }
            if (GameData.allGame_Door1Opened >= 100 && tarots.IsMissionUnlock("Hermit")) {
                tarots.UnlockTarot("Hermit");
            }
            if (GameData.GetTarotCount(GameData.tarotUnlock)>=7 && tarots.IsMissionUnlock("HighPriestess")) {
                tarots.UnlockTarot("HighPriestess");
            }
            */
            //储存永久数据
            SaveManager.SaveForeverData();
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("HangedMan"))) {
                //倒吊人
                //在你完成一个区域的时候（进入传送门的时候）失去当前一半的生命值，并使你的魔力结晶增幅25 %
                Debug.Log("倒吊人触发，失去当前一半的生命值，并使你的魔力结晶增幅25 %");
                GameData.playerHp = (int)(GameData.playerHp * 0.5);
                GameData.gold = (int)(GameData.gold * 1.25);
            }
            LayerChangeTo(GameData.layer + 1,true);
            this.GetComponent<UIManager>().GoTarot();
            audioManagerObject.GetComponent<AudioManager>().PlayBgm(GameData.layer - 1);
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
            if(grid == null) continue;
            Debug.Log(grid.x + " " + grid.y);
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
                        //GameData.map[5,GameData.gridHeight - 1] = grid;
                        grid.MoveTo(5, GameData.gridHeight - 1);
                    } else {
                        //GameData.map[i - 1,GameData.gridHeight - 1] = grid;
                        grid.MoveTo(i - 1,GameData.gridHeight - 1);
                    }
                    //GameData.map[i,GameData.gridHeight - 1] = targetGrid;
                    targetGrid.MoveTo(i,GameData.gridHeight - 1);
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
            GameData.map[mapX,i-1].MoveTo(mapX, i);
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
        //太阳
        //你攻杀的每个怪物使得你获得防御力 + 1，你防杀的每个怪物使得你获得攻击力 + 1
        int pAtk = GameData.playerTotalAtk;
        int pDef = GameData.playerTotalDef;
        if (pAtk - (monster.def + monster.hp) >= 0) {
            Debug.Log("攻杀");
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Sun"))) {
                Debug.Log("太阳触发，获得防御力 + 1");
                GameData.playerDef += 1;
            }
            //如果攻击力大于怪物防御力 + 生命值，那么是攻杀
            if (pDef > monster.atk) {
                Debug.Log("防杀");
                if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Sun"))) {
                    Debug.Log("太阳触发，获得攻击力 + 1");
                    GameData.playerAtk += 1;
                }
                return 0;//如果同时玩家防御力大于怪物攻击力，那么也是防杀
            }
            return 0;
        }
        if (pDef >= monster.atk && pAtk > monster.def) {
            Debug.Log("防杀");
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Sun"))) {
                Debug.Log("太阳触发，获得攻击力 + 1");
                GameData.playerAtk += 1;
            }
            return 0;//如果防御大于怪物攻击力，且玩家攻击力大于怪物防御力，那么是防杀
        }
        return CaculateDamage(monster);
    }


    public int CaculateDamage(GridMonster monster) {
        int pAtk = GameData.playerTotalAtk;
        int pDef = GameData.playerTotalDef;
        if (pAtk - (monster.def + monster.hp) >= 0 ) {
            if (pDef > monster.atk) {
                return 0;
            }
            return 0;
        }
        if(pDef >= monster.atk && pAtk > monster.def) {
            return 0;
        }
        if (pAtk - monster.def <= 0) {
            return -1;//如果玩家攻击力小于怪物防御力，那么显示???
        }
        if(pAtk - monster.def > 0) {
            return (monster.hp / (pAtk - monster.def)) * (monster.atk - pDef);
        }//正常显示伤害
        return -2;//errrrror
    }
    public void UpdatePlayerOffset() {
        GameData.atkOffsetInt = 0;
        GameData.defOffsetInt = 0;
        //战车
        //你具有攻击力 + 1，防御力 + 1
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Chariot"))) {
            GameData.atkOffsetInt += 1;
            GameData.defOffsetInt += 1;
        }
        //月亮
        //你当前持有的每一把钥匙1使得你获得攻击力 + 1，每一把钥匙2使得你获得防御力 + 2
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Moon"))) {
            GameData.atkOffsetInt += GameData.key1;
            GameData.defOffsetInt += GameData.key2 * 2;
        }
        //死神
        //在你击败一个敌人后，如果你未获得死神效果的buff，则在下一次战斗中具有攻击力 + 5
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Death"))) {
            if (GameData.isDeathBuff) {
                GameData.atkOffsetInt += 5;
            }
        }
        GameData.playerTotalAtk = GameData.playerAtk + GameData.atkOffsetInt;
        GameData.playerTotalDef = GameData.playerDef + GameData.defOffsetInt;
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
        tarotEquip = GameData.tarotEquip;
        lastMonsterName = GameData.lastMonsterName;
        isDeathBuff = GameData.isDeathBuff;
        forgeTime = GameData.forgeTime;
        atkOffsetInt = GameData.atkOffsetInt;
        defOffsetInt = GameData.defOffsetInt;
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
