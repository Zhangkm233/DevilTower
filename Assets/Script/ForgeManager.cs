using UnityEngine;

public class ForgeManager : MonoBehaviour
{
    public enum ForgeOpinion {
        HP,ATK,DEF,
    };
    public static readonly int[] forgePrice = {
        10,20,40,70,120,200,330,500,800,900,1000,1100
    };
    public static readonly int[] forgeHp = {
        500,1000,1500,2000,3000
    };
    public static readonly int[] forgeAtk = {
        1,2,4,6,10
    };
    public static readonly int[] forgeDef = {
        2,4,8,12,20
    };
    public GameObject gameManager;
    public static int priceNow = 0;
    public void initializePrice() {
        if (GameData.forgeTime < 12) {
            priceNow = forgePrice[GameData.forgeTime];
        } else {
            priceNow = forgePrice[11];
        }
    }
    public void AffirmForge(ForgeOpinion opinion) {
        if (GameData.gold >= priceNow) {
            GameData.gold -= priceNow;
            switch (opinion) {
                case ForgeOpinion.HP:
                    GameData.playerHp += forgeHp[GameData.layer - 1];
                    break;
                case ForgeOpinion.ATK:
                    GameData.playerAtk += forgeAtk[GameData.layer - 1];
                    break;
                case ForgeOpinion.DEF:
                    GameData.playerDef += forgeDef[GameData.layer - 1];
                    break;
            }
            GameData.forgeTime++;
        }
    }
    public void LeaveForge() {
        gameManager.GetComponent<UIManager>().GoStat();
    }
}
