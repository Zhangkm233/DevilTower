using JetBrains.Annotations;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //������ʾgamedata�ı���
    public int gamelayer;
    public int gamekey1;
    public int gamekey2;
    public int gamekey3;
    public int gameplayerHp;
    public int gameplayerAtk;
    public int gameplayerDef;
    public int gameEventEncountered;

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
            //��¼�������¼�����
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
        if(this.GetComponent<UIManager>().State != UIManager.UIState.STAT) return;
        GameData.layer = layerTo;
        GameData.eventEncounter = 0;
        switch (layerTo) {
            case 1:
                PlayerStatChange(400,10,10,0,0,0,0,0);
                break;
            case 2:
                PlayerStatChange(600,21,21,0,0,0,50,3);
                break;
        }
        //����ͼ��
        catalogObject.GetComponent<MonsterCatalogManager>().UpdateMonsterData();
        this.GetComponent<GridLoader>().LoadMapFromTxt();
        UpdateEachGrid();
    }
    bool MapClickEvent() {
        if (this.GetComponent<UIManager>().State != UIManager.UIState.STAT) return false;
        if (gridTileManager.mapY != GameData.gridHeight-1) return false;
        if (gridTileManager.gridType == Grid.GridType.MONSTER) {
            //��������
            //��ʴ
            GameData.playerHp = (int)(GameData.playerHp * ((GridMonster)GridInMap).CorruptionPercent());
            int battleDamage = ResolveDamage((GridMonster)GridInMap);
            if (battleDamage == -1) {
                Debug.Log("ս���˺���???");
                return false;
            }
            Debug.Log("ս���˺���" + battleDamage);
            GameData.playerHp -= battleDamage;
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
            ClearGridInMap(gridTileManager);
            return true;
        }
        //��Ѫƿ
        if (gridTileManager.gridType == Grid.GridType.BOTTLE) {
            GameData.playerHp += ((GridBottle)gridTileManager.mapGrid).healingPoints;
            Debug.Log("����Ѫƿ��Ѫ��+" + ((GridBottle)gridTileManager.mapGrid).healingPoints);
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
            ClearGridInMap(gridTileManager);
            return true;
        }
        if (gridTileManager.gridType == Grid.GridType.SHOP) {
            this.GetComponent<UIManager>().StartTrade((GridShop)GridInMap,
                gridTileManager.mapX,gridTileManager.mapY);
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
            if (grid.type == Grid.GridType.MONSTER) {
                if (((GridMonster)grid).isStalk) {
                    //׷��
                    Debug.Log(((GridMonster)grid).name + "׷�Դ���");
                    GameData.playerHp -= ((GridMonster)grid).atk - GameData.playerDef;
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
                     targetGrid.type != Grid.GridType.BARRIER) {
                    //����ߵĸ��ӽ���
                    if (i == 0 && ((GridMonster)grid).isLostmind) {
                        GameData.map[5,GameData.gridHeight - 1] = grid;
                    } else {
                        GameData.map[i - 1,GameData.gridHeight - 1] = grid;
                    }
                    GameData.map[i,GameData.gridHeight - 1] = targetGrid;
                    if (((GridMonster)grid).isCrack) {
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
        //����ʱ�õ��˺� ����ɱ�͹�ɱ�ṩ�ط�д
        if (GameData.playerAtk - (monster.def + monster.hp) >= 0) {
            Debug.Log("��ɱ");
            //������������ڹ�������� + ����ֵ����ô�ǹ�ɱ
            if (GameData.playerDef > monster.atk) {
                Debug.Log("��ɱ");
                return 0;//���ͬʱ��ҷ��������ڹ��﹥��������ôҲ�Ƿ�ɱ
            }
            return 0;
        }
        if (GameData.playerDef >= monster.atk && GameData.playerAtk > monster.def) {
            Debug.Log("��ɱ");
            return 0;//����������ڹ��﹥����������ҹ��������ڹ������������ô�Ƿ�ɱ
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
            return -1;//�����ҹ�����С�ڹ������������ô��ʾ???
        }
        if(GameData.playerAtk - monster.def > 0) {
            return (monster.hp / (GameData.playerAtk - monster.def)) * (monster.atk - GameData.playerDef);
        }//������ʾ�˺�
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
