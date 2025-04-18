
using UnityEngine;
using static Grid;


public class GameManager : MonoBehaviour
{
    //������ʾgamedata�ı���
    [Header("GameData������")]
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
            //��¼�������¼�����
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
        //����UI��������
        this.GetComponent<UIManager>().GoStat();
        if (layerTo <= 2) {
            this.GetComponent<UIManager>().goForgeButton.SetActive(false);
        } else {
            this.GetComponent<UIManager>().goForgeButton.SetActive(true);
        }
        //��Ϊ��ͬ�ߴ�ĵ�ͼ���¼���λ�û�Բ��ϸ��ӣ�����Ҫ���¸����и��Ӹ����͸���mapY
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/map" + layerTo + ".txt");
        if(lines.Length != GameData.gridHeight) {
            GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
            foreach (GameObject grid in grids) {
                grid.GetComponent<GridTileManager>().mapY += lines.Length - GameData.gridHeight;
            }
        }
        //���¹������ݵ�ͼ��
        //catalogObject.GetComponent<MonsterCatalogManager>().UpdateMonsterData();
        //���ص�ͼ��map��
        this.GetComponent<GridLoader>().LoadMapFromTxt(); 
        //ˢ�¸���
        UpdateEachGrid();
        //�����Զ��浵
        SaveManager.Save(0);
        //this.GetComponent<UIManager>().GoStat();
    }
    bool MapClickEvent() {
        audioManagerScript = audioManagerObject.GetComponent<AudioManager>();
        
        if (this.GetComponent<UIManager>().State != UIManager.UIState.STAT) {
            Debug.Log("������ȷ��UISTATE��");
            return false;
        }
        if (gridTileManager.mapY != GameData.gridHeight - 1) {
            Debug.Log("������ײ�");
            return false;
        }
        if (gridTileManager.gridType == Grid.GridType.MONSTER) {
            //��������
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
            //��ʴ
            GameData.playerHp = (int)(GameData.playerHp * ((GridMonster)GridInMap).CorruptionPercent());
            int battleDamage = ResolveDamage((GridMonster)GridInMap);
            if (battleDamage == -1) {
                Debug.Log("ս���˺�;???");
                return false;
            }
            Debug.Log("ս���˺�;" + battleDamage);
            audioManagerScript.PlayBattle();
            GetComponent<UIManager>().PopNumber(battleDamage, new Color(255.0f/255f,102.0f/255,28.0f/255));
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Devil"))) {
                Debug.Log("��ħ��������õ�ͬ���˺�������ֵ");
                //��ħ
                //�����ս�����Ϊʹ���õ�ͬ���˺�������ֵ������Ѫƿ���Ϊ������ɵ�ͬ�ڻظ���˫�����˺�
                GameData.playerHp += battleDamage;
            } else {    
                GameData.playerHp -= battleDamage;
            }
            //ȫ�ֽ��
            //��������Ϸ�����У�����5����ħ���������ȶ�ħ���еȶ�ħ���ߵȶ�ħ��
            /*
            if (((GridMonster)GridInMap).name == "���ȶ�ħ" ||
                ((GridMonster)GridInMap).name == "�еȶ�ħ" ||
                ((GridMonster)GridInMap).name == "�ߵȶ�ħ") {
                GameData.allGame_DevilsDefeated++;
            }
            //ɱ��15����ȼ��֮�ꡱ������֮�ꡱ���ߡ��л���ͽ��
            if (((GridMonster)GridInMap).name == "ȼ��֮��" ||
                ((GridMonster)GridInMap).name == "����֮��" ||
                ((GridMonster)GridInMap).name == "�л���ͽ") {
                GameData.allGame_SoulDefeated++;
            }
            if (((GridMonster)GridInMap).name == "�ɻ���ƽ�") {
                GameData.defeatSHDPJ++;
                if (GameData.defeatSHDPJ == 3 && GameData.isEventUsed == false) {
                    if (tarots.IsMissionUnlock("Pope")) tarots.UnlockTarot("Pope");
                }
            }
            if (((GridMonster)GridInMap).name == "�ɻ�������") {
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
                Debug.Log("���д�������õ�ͬ�ڶ�����ֵ�Ĺ������ͷ�����");
                //����
                //��������ͬһ�������ʹ�����õ�ͬ�ڶ�����ֵ�Ĺ������ͷ�����
                if (((GridMonster)GridInMap).name == GameData.lastMonsterName) {
                    GameData.playerAtk += GameData.forgeTime;
                    GameData.playerDef += GameData.forgeTime;
                    GameData.lastMonsterName = ((GridMonster)GridInMap).name;
                    //����Ч��
                }
            }
            //���������buff
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Death"))) {
                if (GameData.isDeathBuff == false) GameData.isDeathBuff = true;
            }
            GameData.gold += ((GridMonster)GridInMap).gold;
            ClearGridInMap(gridTileManager);
            return true;
        }
        //��Կ��
        if (gridTileManager.gridType == Grid.GridType.KEY) {
            switch (gridTileManager.mapGrid.stat) {
                case 1: 
                    GameData.key1++;
                    Debug.Log("������ͭԿ��"); 
                    break;
                case 2: 
                    GameData.key2++;
                    Debug.Log("���˰���Կ��");
                    break;
                case 3: 
                    GameData.key3++;
                    Debug.Log("���˻ƽ�Կ��"); 
                    break;
            }
            audioManagerScript.PlayPick();
            ClearGridInMap(gridTileManager);
            return true;
        }
        //��ʯ
        if (gridTileManager.gridType == Grid.GridType.GEM) {
            if(((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.ATK) {
                GameData.playerAtk += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("���˹�����ʯ��������+" + ((GridGem)gridTileManager.mapGrid).AddSum);
            }
            if (((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.DEF) {
                GameData.playerDef += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("���˷�����ʯ��������+" + ((GridGem)gridTileManager.mapGrid).AddSum);
            }
            audioManagerScript.PlayPick();
            ClearGridInMap(gridTileManager);
            return true;
        }
        //��Ѫƿ
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            int healingPoints = ((GridBottle)gridTileManager.mapGrid).healingPoints;
            //����
            //Ѫƿ�����ṩ25������ֵ
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Star"))) {
                Debug.Log("���Ǵ�����Ѫƿ�����ṩ25������ֵ");
                healingPoints += 25;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Devil"))) {
                Debug.Log("��ħ������Ѫƿ������ɵ�ͬ�ڻظ���˫�����˺�");
                //��ħ
                //Ѫƿ������ɵ�ͬ�ڻظ���˫�����˺�
                GameData.playerHp -= healingPoints * 2;
            } else {
                GameData.playerHp += healingPoints;
            }
            Debug.Log("����Ѫƿ��Ѫ��+" + healingPoints);
            GetComponent<UIManager>().PopNumber(healingPoints, Color.green);
            ClearGridInMap(gridTileManager);
            return true;
        }
        //����
        if (gridTileManager.gridType == Grid.GridType.DOOR) {
            GridDoor door = ((GridDoor)gridTileManager.mapGrid);
            if (door.doorStat == GridDoor.DoorType.BRONZE) {
                if(GameData.key1 > 0) {
                    GameData.key1--;
                    Debug.Log("����" + door.doorStat + "��");
                    //GameData.allGame_Door1Opened++;
                    ClearGridInMap(gridTileManager);
                    return true;
                } else {
                    Debug.Log("��ͭԿ�ײ��㣡");
                    return false;
                }
            }
            if (door.doorStat == GridDoor.DoorType.SILVER) {
                if (GameData.key2 > 0) {
                    GameData.key2--;
                    Debug.Log("����" + door.doorStat + "��");
                    ClearGridInMap(gridTileManager);
                    return true;
                } else {
                    Debug.Log("����Կ�ײ��㣡");
                    return false;
                }
            }
            if (door.doorStat == GridDoor.DoorType.GOLD) {
                if (GameData.key3 > 0) {
                    GameData.key3--;
                    Debug.Log("����" + door.doorStat + "��");
                    ClearGridInMap(gridTileManager);
                    return true;
                } else {
                    Debug.Log("�ƽ�Կ�ײ��㣡");
                    return false;
                }
            }
        }
        if (gridTileManager.gridType == Grid.GridType.NPC) {
            //����
            //�������NPC�¼��󣬻��һ��Կ��1

            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Hermit"))) {
                Debug.Log("���ߴ��������һ��Կ��1");
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
            //����������
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
                    //����map�������Ƿ�û�й�����
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
            //������������
            SaveManager.SaveForeverData();
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("HangedMan"))) {
                //������
                //�������һ�������ʱ�򣨽��봫���ŵ�ʱ��ʧȥ��ǰһ�������ֵ����ʹ���ħ���ᾧ����25 %
                Debug.Log("�����˴�����ʧȥ��ǰһ�������ֵ����ʹ���ħ���ᾧ����25 %");
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
        //���������һ�������ƶ��߼�
        for (int i = 0;i < GameData.gridWidth;i++) {
            //ħ�ĵ��ж� ��ֹ����һ��������
            if (hasLostMind && i == 5) continue;

            Grid grid = GameData.map[i,GameData.gridHeight - 1];
            if(grid == null) continue;
            Debug.Log(grid.x + " " + grid.y);
            if (grid.type == Grid.GridType.MONSTER) {
                if (((GridMonster)grid).isStalk) {
                    if (((GridMonster)grid).stalkTurn == 2) {
                        //׷��
                        Debug.Log(((GridMonster)grid).name + "׷�Դ���");
                        GameData.playerHp -= ((GridMonster)grid).atk - GameData.playerDef;
                        ((GridMonster)grid).stalkTurn = 0;
                    } else {
                        ((GridMonster)grid).stalkTurn++;
                    }
                }
                //�ᶨ
                if (((GridMonster)grid).isFirmness) continue;
                //ħ��
                if (i == 0 && !((GridMonster)grid).isLostmind) continue;
                //����ڵ�һ��������û��ħ�ģ���ô���ƶ�
                Grid targetGrid = null;
                //�����ħ�������ұ߲��ǹ֣���ô�����ƶ������ұ�
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
                    //����ߵĸ��ӽ���
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
                        //����
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
        //����ʱ�õ��˺� ����ɱ�͹�ɱ�ṩ�ط�д
        //̫��
        //�㹥ɱ��ÿ������ʹ�����÷����� + 1�����ɱ��ÿ������ʹ�����ù����� + 1
        int pAtk = GameData.playerTotalAtk;
        int pDef = GameData.playerTotalDef;
        if (pAtk - (monster.def + monster.hp) >= 0) {
            Debug.Log("��ɱ");
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Sun"))) {
                Debug.Log("̫����������÷����� + 1");
                GameData.playerDef += 1;
            }
            //������������ڹ�������� + ����ֵ����ô�ǹ�ɱ
            if (pDef > monster.atk) {
                Debug.Log("��ɱ");
                if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Sun"))) {
                    Debug.Log("̫����������ù����� + 1");
                    GameData.playerAtk += 1;
                }
                return 0;//���ͬʱ��ҷ��������ڹ��﹥��������ôҲ�Ƿ�ɱ
            }
            return 0;
        }
        if (pDef >= monster.atk && pAtk > monster.def) {
            Debug.Log("��ɱ");
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Sun"))) {
                Debug.Log("̫����������ù����� + 1");
                GameData.playerAtk += 1;
            }
            return 0;//����������ڹ��﹥����������ҹ��������ڹ������������ô�Ƿ�ɱ
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
            return -1;//�����ҹ�����С�ڹ������������ô��ʾ???
        }
        if(pAtk - monster.def > 0) {
            return (monster.hp / (pAtk - monster.def)) * (monster.atk - pDef);
        }//������ʾ�˺�
        return -2;//errrrror
    }
    public void UpdatePlayerOffset() {
        GameData.atkOffsetInt = 0;
        GameData.defOffsetInt = 0;
        //ս��
        //����й����� + 1�������� + 1
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Chariot"))) {
            GameData.atkOffsetInt += 1;
            GameData.defOffsetInt += 1;
        }
        //����
        //�㵱ǰ���е�ÿһ��Կ��1ʹ�����ù����� + 1��ÿһ��Կ��2ʹ�����÷����� + 2
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Moon"))) {
            GameData.atkOffsetInt += GameData.key1;
            GameData.defOffsetInt += GameData.key2 * 2;
        }
        //����
        //�������һ�����˺������δ�������Ч����buff��������һ��ս���о��й����� + 5
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
