using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int caculateDamage(GridMonster monster) {
        if(GameData.playerAtk - (monster.def + monster.hp) >= 0 ) {
            return 0;
        }
        if(GameData.playerAtk - monster.def <= 0) {
            return -1;//???
        }
        if(GameData.playerAtk - monster.def > 0) {
            return (monster.hp / (GameData.playerAtk - monster.def)) * (monster.atk - GameData.playerDef);
        }
        return -2;//errrrror
    }

    public GameObject ObjectClick() {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit)) {
                GameObject clickedObject = hit.collider.gameObject;
                return clickedObject;
            }
        }
        return null;
    }
}
