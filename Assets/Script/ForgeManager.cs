using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //public Text forgeDialogText;
    public Text forgeHpText;
    public Text forgeAtkText;
    public Text forgeDefText;
    public TMP_Text goldStatText;
    public TMP_Text Price1, Price2, Price3;
    public static int priceNow = 0;
    public static int hpForgeGive = 0;
    public static int atkForgeGive = 0;
    public static int defForgeGive = 0;
    public void initializePrice() {
        //力量
        //锻造的价格降低10 %

        if (GameData.forgeTime < 12) {
            priceNow = forgePrice[GameData.forgeTime];
        } else {
            priceNow = forgePrice[11];
        }
        if (GameData.IsTarotEquip(gameManager.GetComponent<TarotManager>().TarotToNum("Strength"))) {
            priceNow = Mathf.FloorToInt(priceNow * 0.9f);
        }
        //forgeDialogText.text = "你好，需要锻造吗？只需要" + priceNow + "个魔力结晶";
        hpForgeGive = forgeHp[GameData.layer - 1];
        atkForgeGive = forgeAtk[GameData.layer - 1];
        defForgeGive = forgeDef[GameData.layer - 1];
        forgeHpText.text = "增加\n" +hpForgeGive.ToString() + "\nHP";
        forgeAtkText.text = "增加\n" + atkForgeGive.ToString() + "\nATK";
        forgeDefText.text = "增加\n" + defForgeGive.ToString() + "\nDEF";
        Debug.Log(priceNow);
        Price1.text = priceNow.ToString();
        Price2.text = priceNow.ToString();
        Price3.text = priceNow.ToString();
    }
    public void Update() {
        if (this.isActiveAndEnabled) goldStatText.text = (GameData.gold).ToString();
    }
    public void ForgeHp() {
        AffirmForge(ForgeOpinion.HP);
    }
    public void ForgeAtk() {
        AffirmForge(ForgeOpinion.ATK);
    }
    public void ForgeDef() {
        AffirmForge(ForgeOpinion.DEF);
    }
    public void AffirmForge(ForgeOpinion opinion) {
        if (GameData.gold >= priceNow) {
            GameData.gold -= priceNow;
            switch (opinion) {
                case ForgeOpinion.HP:
                    GameData.playerHp += hpForgeGive;
                    break;
                case ForgeOpinion.ATK:
                    GameData.playerAtk += atkForgeGive;
                    break;
                case ForgeOpinion.DEF:
                    GameData.playerDef += defForgeGive;
                    break;
            }
            GameData.forgeTime++;
        }
        initializePrice();
    }
    public void LeaveForge() {
        gameManager.GetComponent<UIManager>().GoStat();
    }
}
