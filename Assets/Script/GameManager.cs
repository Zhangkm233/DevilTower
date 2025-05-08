using System;
using System.Net;
using Unity.VisualScripting;
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
    public float sfxVolume;
    public float bgmVolume;

    [Space(15)]
    public Camera mainCamera;
    public GameObject objectClick;
    public GameObject catalogObject;
    public GridTileManager gridTileManager;
    public Grid GridInMap;
    public GameObject audioManagerObject;
    public AudioManager audioManagerScript;
    public GameObject backGroundObject;

    void Update() {
        objectClick = ObjectClick();
        if (objectClick != null && (this.GetComponent<UIManager>().State == UIManager.UIState.STAT)) {
            if (objectClick.CompareTag("gridGameObject")) {
                gridTileManager = objectClick.GetComponent<GridTileManager>();
                GridInMap = GameData.map[gridTileManager.mapX,gridTileManager.mapY];
            }
        }
        if (Input.GetMouseButtonDown(0) && objectClick != null) {
            if (objectClick.CompareTag("gridGameObject")) {
                if (MapClickEvent()) {
                    EventCountered();
                    CheckGameOver();
                }
            }
        }
        UpdatePlayerOffset();
        UpdateGameDataToPublic();
    }
    private void Start() {
        StartGame();
    }
    public void StartGame() {
        GameData.layer = 1;
        this.GetComponent<GridLoader>().InitialzeMapAndGrid();
        this.GetComponent<UIManager>().InitializeUI();
        SaveManager.LoadForeverData();
        LayerChangeTo(GameData.layer,false);
        if (MenuData.isContinueGame) {
            GameData.saveSlotChoose = MenuData.loadGameSlot;
            this.GetComponent<GridLoader>().LoadAll();
            SaveManager.Load(MenuData.loadGameSlot);
            this.GetComponent<GameManager>().UpdateEachGrid();
        } else {
            this.GetComponent<UIManager>().GoDialog();
            this.GetComponent<DialogManager>().ReadDialog(0,GameData.layer);
        }
        audioManagerObject.GetComponent<AudioManager>().InitialVolume();
        audioManagerObject.GetComponent<AudioManager>().PlayBgm(GameData.layer - 1);
    }
    public void RestartGame() {
        SaveManager.Load(0);
        this.GetComponent<GameManager>().UpdateEachGrid();
    }
    public void CheckGameOver() {
        if (GameData.playerHp <= 0) {
            Debug.Log("Game Over");
            audioManagerObject.GetComponent<AudioManager>().PlayBadEndBGM();
            this.GetComponent<UIManager>().FadeAndGoState(UIManager.UIState.FAIL,0.2f);
        }
    }

    public void EventCountered() {
        MonsterMovement();
        GameData.eventEncounter++;
        //命运之轮
        //你每完成一个事件，有3 % 的概率获得随机一项效果：
        //*攻击力 + 3 %
        //*防御力 + 3 %
        //*血量 + 3 %
        //*魔力结晶 + 33 %
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("WheelOfFortune"))) {
            int randomNumber = UnityEngine.Random.Range(0,100);
            Debug.Log("命运之轮判定" + randomNumber);
            if (randomNumber < 3) {
                randomNumber = UnityEngine.Random.Range(0,4);
                switch (randomNumber) {
                    case 0:
                        Debug.Log("命运之轮触发，攻击力+ 3 %");
                        GameData.playerAtk += (int)Math.Ceiling(GameData.playerAtk * 0.03f);
                        break;
                    case 1:
                        Debug.Log("命运之轮触发，防御力+ 3 %");
                        GameData.playerDef += (int)Math.Ceiling(GameData.playerDef * 0.03f);
                        break;
                    case 2:
                        Debug.Log("命运之轮触发，血量+ 3 %");
                        GameData.playerHp += (int)Math.Ceiling(GameData.playerHp * 0.03f);
                        break;
                    case 3:
                        Debug.Log("命运之轮触发，魔力结晶+ 33 %");
                        GameData.gold += (int)Math.Ceiling(GameData.gold * 0.33f);
                        break;
                }
            }
        }
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
    public void SaveForeverData() {
        SaveManager.SaveForeverData();
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
                    PlayerStatChange(100,5,5,0,0,0,0,0);
                    break;
                case 2:
                    PlayerStatChange(500,5,5,0,0,0,0,0);
                    break;
                case 3:
                    PlayerStatChange(800,10,10,1,1,0,0,0);
                    break;
                case 4:
                    PlayerStatChange(1000,20,20,1,1,0,0,0);
                    break;
                case 5:
                    PlayerStatChange(1000,45,45,1,1,0,0,0);
                    break;
                case 6:
                    PlayerStatChange(1000, 75, 75, 1, 1, 0, 0, 0);
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
        catalogObject.GetComponent<MonsterCatalogManager>().UpdateMonsterData();
        //加载地图到map里
        this.GetComponent<GridLoader>().LoadMapFromTxt();
        UpdateLayerSprites();
        //刷新格子
        //UpdateEachGrid();
        //更新背景sprite
        //backGroundObject.GetComponent<BackGroundManager>().UpdateSprite();
        //更新自动存档
        SaveManager.Save(0);
        //this.GetComponent<UIManager>().GoStat();
        //audioManagerObject.GetComponent<AudioManager>().PlayBgm(GameData.layer - 1);
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
            int playerOldHp = GameData.playerHp;
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
                Debug.Log("审判判断，连续击败同一个怪物会使得你获得2点攻击力和防御力");
                //审判
                //连续击败同一个怪物会使得你获得2点攻击力和防御力
                if (((GridMonster)GridInMap).name == GameData.lastMonsterName) {
                    Debug.Log("审判触发，获得2点攻击力和防御力");
                    GameData.playerAtk += 2;
                    GameData.playerDef += 2;
                    GameData.lastMonsterName = ((GridMonster)GridInMap).name;
                    //触发效果
                }
            }
            //重置死神的buff
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Death"))) {
                GameData.isDeathBuff = !GameData.isDeathBuff;
            }
            GameData.gold += ((GridMonster)GridInMap).gold;
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("HighPriestess"))) {
                Debug.Log("女祭司触发，在你击败一个怪物的时候，额外获得2魔力结晶");
                GameData.gold += 2;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Hermit"))) {
                Debug.Log("隐者判断，如果你在一次战斗中至少失去了25%的生命值，获得一把钥匙1");
                if(playerOldHp - GameData.playerHp >= playerOldHp * 0.25) {
                    Debug.Log("隐者触发，获得一把钥匙1");
                    GameData.key1++;
                }
            }
            //正义 每当你击败具有“魔心”的怪物后，会使你获得500生命值
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Justice"))) {
                Debug.Log("正义判断，每当你击败具有“魔心”的怪物后，会使你获得500生命值");
                if (((GridMonster)GridInMap).isLostmind) {
                    Debug.Log("正义触发，获得500生命值");
                    GameData.playerHp += 500;
                }
            }
            if (GameData.popeBuffTime != 0) {
                GameData.popeBuffTime = 0;
                Debug.Log("教皇提供的临时防御值已归0");
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
            audioManagerScript.PlayPick();
            ClearGridInMap(gridTileManager);
            return true;
        }
        //捡宝石
        if (gridTileManager.gridType == Grid.GridType.GEM) {
            if(((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.ATK) {
                GameData.playerAtk += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("捡到了攻击宝石，攻击力+" + ((GridGem)gridTileManager.mapGrid).AddSum);
                audioManagerScript.PlayAtk();
            }
            if (((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.DEF) {
                GameData.playerDef += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("捡到了防御宝石，防御力+" + ((GridGem)gridTileManager.mapGrid).AddSum);
                audioManagerScript.PlayDef();
            }
            //audioManagerScript.PlayPick();
            ClearGridInMap(gridTileManager);
            return true;
        }
        //捡血瓶
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            int healingPoints = ((GridBottle)gridTileManager.mapGrid).healingPoints;
            //星星
            //血瓶额外提供10点生命值
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Star"))) {
                Debug.Log("星星触发，血瓶额外提供10点生命值");
                healingPoints += 10;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Empress"))) {
                Debug.Log("女皇触发，你从血瓶中会额外获得150点生命值，但是在你获得一个血瓶后，立刻摧毁其相邻的事件");
                healingPoints += 150;
            }
            //教皇
            //每当你拾取血瓶，你获得在下一次战斗中具有防御力 + 1（可叠加）
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Pope"))) {
                Debug.Log("教皇触发，获得在下一次战斗中具有防御力 + 1（可叠加）");
                GameData.popeBuffTime++;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Devil"))) {
                Debug.Log("恶魔触发，血瓶对你造成等同于回复量双倍的伤害");
                //恶魔
                //血瓶对你造成等同于回复量双倍的伤害
                healingPoints *= 2;
                GameData.playerHp -= healingPoints;
                GetComponent<UIManager>().PopNumber(healingPoints,Color.red);
            } else {
                Debug.Log("捡到了血瓶，血量+" + healingPoints);
                GameData.playerHp += healingPoints;
                GetComponent<UIManager>().PopNumber(healingPoints,Color.green);
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Empress"))) {
                Debug.Log("女皇触发，在你获得一个血瓶后，立刻摧毁其相邻的事件");
                if (gridTileManager.mapX != 0) ClearGridInMap(gridTileManager.mapX - 1,gridTileManager.mapY);
                if (gridTileManager.mapX != 5) ClearGridInMap(gridTileManager.mapX + 1,gridTileManager.mapY);
            }
            audioManagerScript.PlayHeal();
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
                    audioManagerScript.PlayDoor();
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
                    audioManagerScript.PlayDoor();
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
                    audioManagerScript.PlayDoor();
                    return true;
                } else {
                    Debug.Log("黄金钥匙不足！");
                    return false;
                }
            }
        }
        if (gridTileManager.gridType == Grid.GridType.NPC) {
            /*
            if (GameData.layer > 4) {
                ClearGridInMap(gridTileManager);
                return true;
            }*/
            int dialogStat = 114514;
            switch (GameData.layer) {
                case 1:
                case 2:
                    dialogStat = GameData.GetRandomNumberExclude(0,4,GameData.npcEncounteredLayer12);
                    for(int i = 0;i < GameData.npcEncounteredLayer12.Length;i++) {
                        if(GameData.npcEncounteredLayer12[i] == -1 ) {
                            GameData.npcEncounteredLayer12[i] = dialogStat;
                        }
                    }
                    break;
                case 3:
                case 4:
                    dialogStat = GameData.GetRandomNumberExclude(0,4,GameData.npcEncounteredLayer34);
                    Debug.Log("NPC对话" + dialogStat);
                    for (int i = 0;i < GameData.npcEncounteredLayer34.Length;i++) {
                        if (GameData.npcEncounteredLayer34[i] == -1) {
                            GameData.npcEncounteredLayer34[i] = dialogStat;
                            break;
                        }
                    }
                    break;
                case 5:
                case 6:
                    dialogStat = GameData.GetRandomNumberExclude(0,4,GameData.npcEncounteredLayer56);
                    for (int i = 0;i < GameData.npcEncounteredLayer56.Length;i++) {
                        if (GameData.npcEncounteredLayer56[i] == -1) {
                            GameData.npcEncounteredLayer56[i] = dialogStat;
                        }
                    }
                    break;
            }
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
            //储存永久数据
            SaveManager.SaveForeverData();
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("HangedMan"))) {
                //倒吊人
                //在你完成一个区域的时候（进入传送门的时候）失去当前一半的生命值，并使你的魔力结晶增幅25 %
                Debug.Log("倒吊人触发，失去当前一半的生命值，并使你的魔力结晶增幅25 %");
                GameData.playerHp = (int)(GameData.playerHp * 0.5);
                GameData.gold = (int)(GameData.gold * 1.25);
            }
            //进入塔罗牌
            audioManagerScript.PlayTeleport();
            this.GetComponent<UIManager>().StartDialog(10,GameData.layer);
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
            //Debug.Log(grid.x + " " + grid.y);
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Tower"))) {
                //塔
                //门1和门2会像怪物一样移动，在你将要进入一个新的区域时，立刻获得一把钥匙1和一把钥匙2
                if (grid.type == Grid.GridType.DOOR && ((grid.stat == 1) || (grid.stat == 2))) {
                    if (i == 0) continue; 
                    Grid targetGrid = GameData.map[i - 1,GameData.gridHeight - 1];
                    if (targetGrid.type != Grid.GridType.MONSTER && targetGrid.type != Grid.GridType.DOOR &&
                         targetGrid.type != Grid.GridType.BARRIER && targetGrid.type != Grid.GridType.PORTAL) {
                        //与左边的格子交换
                        if (i == 0 && ((GridMonster)grid).isLostmind) {
                            grid.MoveTo(5,GameData.gridHeight - 1);
                        } else {
                            grid.MoveTo(i - 1,GameData.gridHeight - 1);
                        }
                        targetGrid.MoveTo(i,GameData.gridHeight - 1);
                    }
                    continue;
                }
            }
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
                try {
                    Grid.GridType gridType = targetGrid.type;
                } catch (System.Exception e) {
                    Debug.Log("怪物移动异常" + e);
                    return;
                }
                Debug.Log("怪物移动");
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
    public void UpdateLayerSprites() {
        UpdateEachGrid();
        backGroundObject.GetComponent<BackGroundManager>().UpdateSprite();
        audioManagerObject.GetComponent<AudioManager>().PlayBgm(GameData.layer - 1);
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
        int mDef = monster.def; 
        //节制 无视怪兽防御
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Temperance"))) {
            mDef = 0;
        }
        if (pAtk - (mDef + monster.hp) >= 0) {
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
        if (pDef >= monster.atk && pAtk > mDef) {
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
        int mDef = monster.def; 
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Temperance"))) {
            mDef = 0;
        }
        if (pAtk - (mDef + monster.hp) >= 0 ) {
            if (pDef > monster.atk) {
                return 0;
            }
            return 0;
        }
        if(pDef >= monster.atk && pAtk > mDef) {
            return 0;
        }
        if (pAtk - mDef <= 0) {
            return -1;//如果玩家攻击力小于怪物防御力，那么显示???
        }
        if(pAtk - mDef > 0) {
            return (monster.hp / (pAtk - mDef)) * (monster.atk - pDef);
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
        //教皇的buff 每当你拾取血瓶，你获得在下一次战斗中具有防御力+1（可叠加）
        if (GameData.popeBuffTime != 0) {
            GameData.defOffsetInt += GameData.popeBuffTime;
        }
        //节制
        //你的攻击力降低80 %，但是你无视敌方防御
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Temperance"))) {
            //向上取整
            GameData.playerTotalAtk = (int)Math.Ceiling(((GameData.playerAtk + GameData.atkOffsetInt) * 0.2));
        } else {
            GameData.playerTotalAtk = GameData.playerAtk + GameData.atkOffsetInt;
        }
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
        bgmVolume = GameData.bgmVolume;
        sfxVolume = GameData.sfxVolume;

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
