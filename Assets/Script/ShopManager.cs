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
            shopStat.text = "������" + itemGiveOutNum + "��" + Hanize(itemGiveOut) +
                "�������" + itemExchangeForNum + "��" + Hanize(itemExchangeFor);
        } else {
            isThisShopInfinite = true;
            shopStat.text = "������" + itemGiveOutNum + "��" + Hanize(itemGiveOut) +
                "�������" + itemExchangeForNum + "��" + Hanize(itemExchangeFor) + "���������޵�";
        }
    }
    public string Hanize(string str) {
        //����
        switch (str) {
            case "gold":
                return "ħ���ᾧ";
            case "key1":
                return "��ͭԿ��";
            case "key2":
                return "����Կ��";
            case "key3":
                return "�ƽ�Կ��";
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
