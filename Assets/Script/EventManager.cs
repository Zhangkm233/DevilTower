
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //�������������¼�
    public GridEvent.EventType eventType = GridEvent.EventType.NULL;
    public GameObject GameManager;
    public int mapX;
    public int mapY;
    private void Update() {
        if(GameManager.GetComponent<UIManager>().State != UIManager.UIState.EVENT) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CancelEvent();
        }
        if(eventType == GridEvent.EventType.SOULARROW) {
            if (Input.GetMouseButtonDown(0) && GameManager.GetComponent<GameManager>().gridTileManager.gridType == Grid.GridType.MONSTER) {
                ArrowClickOnMonster(GameManager.GetComponent<GameManager>().gridTileManager);
            }
        }
        if (eventType == GridEvent.EventType.SOULGATE) {
            if (Input.GetMouseButtonDown(0) && GameManager.GetComponent<GameManager>().gridTileManager.gridType == Grid.GridType.MONSTER) {
                DoorClickOnMonster(GameManager.GetComponent<GameManager>().gridTileManager);
            }
        }
    }
    public void ArrowClickOnMonster(GridTileManager cgtManager) {
        if (((GridMonster)GameData.map[cgtManager.mapX,cgtManager.mapY]).isBoss == false) {
            //GameData.gold += ((GridMonster)GameData.map[cgtManager.mapX,cgtManager.mapY]).gold; ����˵�������
            GameManager.GetComponent<GameManager>().ClearGridInMap(cgtManager);
            if (cgtManager.mapX == mapX && cgtManager.mapY > mapY) {
                GameManager.GetComponent<GameManager>().ClearGridInMap(mapX,mapY + 1);
            } else {
                GameManager.GetComponent<GameManager>().ClearGridInMap(mapX,mapY);
            }
            GameManager.GetComponent<GameManager>().MonsterMovement();
            GameData.eventEncounter++;
            GameManager.GetComponent<UIManager>().GoStat();
        } else {
            Debug.Log("ɱ����boss");
        }
    }
    public void DoorClickOnMonster(GridTileManager cgtManager) {
        if (cgtManager.mapY != GameData.gridHeight - 1) { 
            Debug.Log("���ڵ�һ��");
            return; 
        }
        if (GameData.map[cgtManager.mapX,cgtManager.mapY - 2] == null) {
            Debug.Log("����λ��û�ж���");
            return;
        }
        Grid tempGrid = GameData.map[cgtManager.mapX,cgtManager.mapY - 2];
        GameData.map[cgtManager.mapX,cgtManager.mapY - 2] = GameData.map[cgtManager.mapX,cgtManager.mapY];
        GameData.map[cgtManager.mapX,cgtManager.mapY] = tempGrid;
        GameManager.GetComponent<GameManager>().ClearGridInMap(mapX,mapY); 
        GameManager.GetComponent<GameManager>().MonsterMovement();
        GameData.eventEncounter++;
        GameManager.GetComponent<UIManager>().GoStat();
    }
    public void CancelEvent() {
        GameManager.GetComponent<UIManager>().GoStat();
    }
}
