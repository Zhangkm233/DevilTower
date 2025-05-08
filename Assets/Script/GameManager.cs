using System;
using System.Net;
using Unity.VisualScripting;
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
        //����֮��
        //��ÿ���һ���¼�����3 % �ĸ��ʻ�����һ��Ч����
        //*������ + 3 %
        //*������ + 3 %
        //*Ѫ�� + 3 %
        //*ħ���ᾧ + 33 %
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("WheelOfFortune"))) {
            int randomNumber = UnityEngine.Random.Range(0,100);
            Debug.Log("����֮���ж�" + randomNumber);
            if (randomNumber < 3) {
                randomNumber = UnityEngine.Random.Range(0,4);
                switch (randomNumber) {
                    case 0:
                        Debug.Log("����֮�ִ�����������+ 3 %");
                        GameData.playerAtk += (int)Math.Ceiling(GameData.playerAtk * 0.03f);
                        break;
                    case 1:
                        Debug.Log("����֮�ִ�����������+ 3 %");
                        GameData.playerDef += (int)Math.Ceiling(GameData.playerDef * 0.03f);
                        break;
                    case 2:
                        Debug.Log("����֮�ִ�����Ѫ��+ 3 %");
                        GameData.playerHp += (int)Math.Ceiling(GameData.playerHp * 0.03f);
                        break;
                    case 3:
                        Debug.Log("����֮�ִ�����ħ���ᾧ+ 33 %");
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
        catalogObject.GetComponent<MonsterCatalogManager>().UpdateMonsterData();
        //���ص�ͼ��map��
        this.GetComponent<GridLoader>().LoadMapFromTxt();
        UpdateLayerSprites();
        //ˢ�¸���
        //UpdateEachGrid();
        //���±���sprite
        //backGroundObject.GetComponent<BackGroundManager>().UpdateSprite();
        //�����Զ��浵
        SaveManager.Save(0);
        //this.GetComponent<UIManager>().GoStat();
        //audioManagerObject.GetComponent<AudioManager>().PlayBgm(GameData.layer - 1);
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
            int playerOldHp = GameData.playerHp;
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
                Debug.Log("�����жϣ���������ͬһ�������ʹ������2�㹥�����ͷ�����");
                //����
                //��������ͬһ�������ʹ������2�㹥�����ͷ�����
                if (((GridMonster)GridInMap).name == GameData.lastMonsterName) {
                    Debug.Log("���д��������2�㹥�����ͷ�����");
                    GameData.playerAtk += 2;
                    GameData.playerDef += 2;
                    GameData.lastMonsterName = ((GridMonster)GridInMap).name;
                    //����Ч��
                }
            }
            //���������buff
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Death"))) {
                GameData.isDeathBuff = !GameData.isDeathBuff;
            }
            GameData.gold += ((GridMonster)GridInMap).gold;
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("HighPriestess"))) {
                Debug.Log("Ů��˾�������������һ�������ʱ�򣬶�����2ħ���ᾧ");
                GameData.gold += 2;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Hermit"))) {
                Debug.Log("�����жϣ��������һ��ս��������ʧȥ��25%������ֵ�����һ��Կ��1");
                if(playerOldHp - GameData.playerHp >= playerOldHp * 0.25) {
                    Debug.Log("���ߴ��������һ��Կ��1");
                    GameData.key1++;
                }
            }
            //���� ÿ������ܾ��С�ħ�ġ��Ĺ���󣬻�ʹ����500����ֵ
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Justice"))) {
                Debug.Log("�����жϣ�ÿ������ܾ��С�ħ�ġ��Ĺ���󣬻�ʹ����500����ֵ");
                if (((GridMonster)GridInMap).isLostmind) {
                    Debug.Log("���崥�������500����ֵ");
                    GameData.playerHp += 500;
                }
            }
            if (GameData.popeBuffTime != 0) {
                GameData.popeBuffTime = 0;
                Debug.Log("�̻��ṩ����ʱ����ֵ�ѹ�0");
            }
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
                audioManagerScript.PlayAtk();
            }
            if (((GridGem)gridTileManager.mapGrid).gemType == GridGem.GemType.DEF) {
                GameData.playerDef += ((GridGem)gridTileManager.mapGrid).AddSum;
                Debug.Log("���˷�����ʯ��������+" + ((GridGem)gridTileManager.mapGrid).AddSum);
                audioManagerScript.PlayDef();
            }
            //audioManagerScript.PlayPick();
            ClearGridInMap(gridTileManager);
            return true;
        }
        //��Ѫƿ
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            int healingPoints = ((GridBottle)gridTileManager.mapGrid).healingPoints;
            //����
            //Ѫƿ�����ṩ10������ֵ
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Star"))) {
                Debug.Log("���Ǵ�����Ѫƿ�����ṩ10������ֵ");
                healingPoints += 10;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Empress"))) {
                Debug.Log("Ů�ʴ��������Ѫƿ�л������150������ֵ������������һ��Ѫƿ�����̴ݻ������ڵ��¼�");
                healingPoints += 150;
            }
            //�̻�
            //ÿ����ʰȡѪƿ����������һ��ս���о��з����� + 1���ɵ��ӣ�
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Pope"))) {
                Debug.Log("�̻ʴ������������һ��ս���о��з����� + 1���ɵ��ӣ�");
                GameData.popeBuffTime++;
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Devil"))) {
                Debug.Log("��ħ������Ѫƿ������ɵ�ͬ�ڻظ���˫�����˺�");
                //��ħ
                //Ѫƿ������ɵ�ͬ�ڻظ���˫�����˺�
                healingPoints *= 2;
                GameData.playerHp -= healingPoints;
                GetComponent<UIManager>().PopNumber(healingPoints,Color.red);
            } else {
                Debug.Log("����Ѫƿ��Ѫ��+" + healingPoints);
                GameData.playerHp += healingPoints;
                GetComponent<UIManager>().PopNumber(healingPoints,Color.green);
            }
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Empress"))) {
                Debug.Log("Ů�ʴ�����������һ��Ѫƿ�����̴ݻ������ڵ��¼�");
                if (gridTileManager.mapX != 0) ClearGridInMap(gridTileManager.mapX - 1,gridTileManager.mapY);
                if (gridTileManager.mapX != 5) ClearGridInMap(gridTileManager.mapX + 1,gridTileManager.mapY);
            }
            audioManagerScript.PlayHeal();
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
                    audioManagerScript.PlayDoor();
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
                    audioManagerScript.PlayDoor();
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
                    audioManagerScript.PlayDoor();
                    return true;
                } else {
                    Debug.Log("�ƽ�Կ�ײ��㣡");
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
                    Debug.Log("NPC�Ի�" + dialogStat);
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
            //������������
            SaveManager.SaveForeverData();
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("HangedMan"))) {
                //������
                //�������һ�������ʱ�򣨽��봫���ŵ�ʱ��ʧȥ��ǰһ�������ֵ����ʹ���ħ���ᾧ����25 %
                Debug.Log("�����˴�����ʧȥ��ǰһ�������ֵ����ʹ���ħ���ᾧ����25 %");
                GameData.playerHp = (int)(GameData.playerHp * 0.5);
                GameData.gold = (int)(GameData.gold * 1.25);
            }
            //����������
            audioManagerScript.PlayTeleport();
            this.GetComponent<UIManager>().StartDialog(10,GameData.layer);
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
            //Debug.Log(grid.x + " " + grid.y);
            if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Tower"))) {
                //��
                //��1����2�������һ���ƶ������㽫Ҫ����һ���µ�����ʱ�����̻��һ��Կ��1��һ��Կ��2
                if (grid.type == Grid.GridType.DOOR && ((grid.stat == 1) || (grid.stat == 2))) {
                    if (i == 0) continue; 
                    Grid targetGrid = GameData.map[i - 1,GameData.gridHeight - 1];
                    if (targetGrid.type != Grid.GridType.MONSTER && targetGrid.type != Grid.GridType.DOOR &&
                         targetGrid.type != Grid.GridType.BARRIER && targetGrid.type != Grid.GridType.PORTAL) {
                        //����ߵĸ��ӽ���
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
                try {
                    Grid.GridType gridType = targetGrid.type;
                } catch (System.Exception e) {
                    Debug.Log("�����ƶ��쳣" + e);
                    return;
                }
                Debug.Log("�����ƶ�");
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
        //����ʱ�õ��˺� ����ɱ�͹�ɱ�ṩ�ط�д
        //̫��
        //�㹥ɱ��ÿ������ʹ�����÷����� + 1�����ɱ��ÿ������ʹ�����ù����� + 1
        int pAtk = GameData.playerTotalAtk;
        int pDef = GameData.playerTotalDef;
        int mDef = monster.def; 
        //���� ���ӹ��޷���
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Temperance"))) {
            mDef = 0;
        }
        if (pAtk - (mDef + monster.hp) >= 0) {
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
        if (pDef >= monster.atk && pAtk > mDef) {
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
            return -1;//�����ҹ�����С�ڹ������������ô��ʾ???
        }
        if(pAtk - mDef > 0) {
            return (monster.hp / (pAtk - mDef)) * (monster.atk - pDef);
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
        //�̻ʵ�buff ÿ����ʰȡѪƿ����������һ��ս���о��з�����+1���ɵ��ӣ�
        if (GameData.popeBuffTime != 0) {
            GameData.defOffsetInt += GameData.popeBuffTime;
        }
        //����
        //��Ĺ���������80 %�����������ӵз�����
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Temperance"))) {
            //����ȡ��
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
