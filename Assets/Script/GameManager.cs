using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject objectClick;
    void Update() {
        objectClick = ObjectClick();
        if (Input.GetMouseButtonDown(0)) {
            GridTileManager gridTileManager = objectClick.GetComponent<GridTileManager>();

            if (gridTileManager.gridType == Grid.GridType.MONSTER) {
                GridMonster monster = (GridMonster)GameData.map[objectClick.GetComponent<GridTileManager>().mapX,objectClick.GetComponent<GridTileManager>().mapY];
                Debug.Log("ս���˺���"+caculateDamage(monster));
            }
            if (gridTileManager.gridType == Grid.GridType.KEY || gridTileManager.gridType == Grid.GridType.DOOR) {

            }
        }
    }
    public int caculateDamage(GridMonster monster) {
        if(GameData.playerAtk - (monster.def + monster.hp) >= 0 ) {
            //������������ڹ�������� + ����ֵ����ô�ǹ�ɱ
            if (GameData.playerDef > monster.atk) {
                return 0;//���ͬʱ��ҷ��������ڹ��﹥��������ôҲ�Ƿ�ɱ
            }
            return 0;
        }
        if(GameData.playerDef >= monster.atk && GameData.playerAtk > monster.def) {
            return 0;//����������ڹ��﹥����������ҹ��������ڹ������������ô�Ƿ�ɱ
        }
        if (GameData.playerAtk - monster.def <= 0) {
            return -1;//�����ҹ�����С�ڹ������������ô��ʾ???
        }
        if(GameData.playerAtk - monster.def > 0) {
            return (monster.hp / (GameData.playerAtk - monster.def)) * (monster.atk - GameData.playerDef);
        }//������ʾ�˺�
        return -2;//errrrror
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
