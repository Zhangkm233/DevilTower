using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public GameObject gameManager;
    public Text shopStat;
    public string itemGiveOut;
    public string itemExchangeFor;
    public int mapX, mapY;
    public int itemGiveOutNum;
    public int itemExchangeForNum;
    public bool isThisShopInfinite = false;

    public void UpdateShopData(GridShop gridShop,int gridX,int gridY) {
        mapX = gridX; mapY = gridY;
        itemGiveOut = gridShop.itemGiveOut;
        itemExchangeFor = gridShop.itemExchangeFor;
        itemGiveOutNum = gridShop.itemGiveOutNum;
        itemExchangeForNum = gridShop.itemExchangeForNum;
        if (!gridShop.isInfinite) {
            shopStat.text = "我想用" + itemGiveOutNum + "个" + Hanize(itemGiveOut) +
                "交换你的" + itemExchangeForNum + "个" + Hanize(itemExchangeFor);
        } else {
            isThisShopInfinite = true;
            shopStat.text = "我想用" + itemGiveOutNum + "个" + Hanize(itemGiveOut) +
                "交换你的" + itemExchangeForNum + "个" + Hanize(itemExchangeFor) + "，这是无限的";
        }
    }
    public string Hanize(string str) {
        //汉化
        switch (str) {
            case "gold":
                return "魔力结晶";
            case "key1":
                return "青铜钥匙";
            case "key2":
                return "白银钥匙";
            case "key3":
                return "黄金钥匙";
            default:
                break;
        }
        return "?";
    }
    public bool IsEnough(string str,int num) {
        switch (str) {
            case "gold":
                if(GameData.gold >= num) return true;
                return false;
            case "key1":
                if (GameData.key1 >= num) return true;
                return false;
            case "key2":
                if (GameData.key2 >= num) return true;
                return false;
            case "key3":
                if (GameData.key3 >= num) return true;
                return false;
        }
        return false;
    }

    public void AddToInventory(string str,int num) {
        switch (str) {
            case "gold":
                GameData.gold += num;
                break;
            case "key1":
                GameData.key1 += num;
                break;
            case "key2":
                GameData.key2 += num;
                break;
            case "key3":
                GameData.key3 += num;
                break;
        }
    }
    public void AffirmTrade() {
        if (IsEnough(itemExchangeFor,itemExchangeForNum)) {
            GameData.gold -= itemExchangeForNum;
            AddToInventory(itemGiveOut,itemGiveOutNum);
            gameManager.GetComponent<GameManager>().ClearGridInMap(mapX,mapY);
            if(!isThisShopInfinite) {
                gameManager.GetComponent<GameManager>().MonsterMovement();
                GameData.eventEncounter++;
                gameManager.GetComponent<UIManager>().GoStat();
            }
        }
    }
    public void LeaveTrade() {
        gameManager.GetComponent<UIManager>().GoStat();
    }
    public void ExileTrade() {
        gameManager.GetComponent<GameManager>().ClearGridInMap(mapX, mapY);
        gameManager.GetComponent<GameManager>().MonsterMovement();
        GameData.eventEncounter++;
        gameManager.GetComponent<UIManager>().GoStat();
    }
}
