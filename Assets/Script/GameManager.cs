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
                Debug.Log("战斗伤害："+caculateDamage(monster));
            }
            if (gridTileManager.gridType == Grid.GridType.KEY || gridTileManager.gridType == Grid.GridType.DOOR) {

            }
        }
    }
    public int caculateDamage(GridMonster monster) {
        if(GameData.playerAtk - (monster.def + monster.hp) >= 0 ) {
            //如果攻击力大于怪物防御力 + 生命值，那么是攻杀
            if (GameData.playerDef > monster.atk) {
                return 0;//如果同时玩家防御力大于怪物攻击力，那么也是防杀
            }
            return 0;
        }
        if(GameData.playerDef >= monster.atk && GameData.playerAtk > monster.def) {
            return 0;//如果防御大于怪物攻击力，且玩家攻击力大于怪物防御力，那么是防杀
        }
        if (GameData.playerAtk - monster.def <= 0) {
            return -1;//如果玩家攻击力小于怪物防御力，那么显示???
        }
        if(GameData.playerAtk - monster.def > 0) {
            return (monster.hp / (GameData.playerAtk - monster.def)) * (monster.atk - GameData.playerDef);
        }//正常显示伤害
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
