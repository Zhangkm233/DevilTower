using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject itemImage;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    public string itemGiveOut;
    public string itemExchangeFor;
    public int mapX, mapY;
    public int itemGiveOutNum;
    public int itemExchangeForNum;
    public bool isThisShopInfinite = false;
    public SpriteScriptObject[] layersprites;

    public void UpdateShopData(GridShop gridShop,int gridX,int gridY) {
        mapX = gridX; mapY = gridY;
        isThisShopInfinite = gridShop.isInfinite;
        itemGiveOut = gridShop.itemGiveOut;
        itemExchangeFor = gridShop.itemExchangeFor;
        itemGiveOutNum = gridShop.itemGiveOutNum;
        itemExchangeForNum = gridShop.itemExchangeForNum;
        itemName.text = Hanize(itemGiveOut) + "*" + itemGiveOutNum;
        itemPrice.text = itemExchangeForNum.ToString();
        switch (itemGiveOut) {
            case "key1":
                if (GameData.layer - 1 >= layersprites.Length) {
                    itemImage.GetComponent<Image>().sprite = layersprites[0].spriteData[7].sprites[0];
                    break;
                }
                itemImage.GetComponent<Image>().sprite = layersprites[GameData.layer - 1].spriteData[7].sprites[0];
                break;
            case "key2":
                if (GameData.layer - 1 >= layersprites.Length) {
                    itemImage.GetComponent<Image>().sprite = layersprites[0].spriteData[7].sprites[1];
                    break;
                }
                itemImage.GetComponent<Image>().sprite = layersprites[GameData.layer - 1].spriteData[7].sprites[1];
                break;
            case "key3":
                if (GameData.layer - 1 >= layersprites.Length) {
                    itemImage.GetComponent<Image>().sprite = layersprites[0].spriteData[7].sprites[2];
                    break;
                }
                itemImage.GetComponent<Image>().sprite = layersprites[GameData.layer - 1].spriteData[7].sprites[2];
                break;
            case "B1":
                if (GameData.layer - 1 >= layersprites.Length) {
                    itemImage.GetComponent<Image>().sprite = layersprites[0].spriteData[0].sprites[0];
                    break;
                }
                itemImage.GetComponent<Image>().sprite = layersprites[GameData.layer - 1].spriteData[0].sprites[0];
                break;
            case "B2":
                if (GameData.layer - 1 >= layersprites.Length) {
                    itemImage.GetComponent<Image>().sprite = layersprites[0].spriteData[0].sprites[1];
                    break;
                }
                itemImage.GetComponent<Image>().sprite = layersprites[GameData.layer - 1].spriteData[0].sprites[1];
                break;
            case "B3":
                if (GameData.layer - 1 >= layersprites.Length) {
                    itemImage.GetComponent<Image>().sprite = layersprites[0].spriteData[0].sprites[2];
                    break;
                }
                itemImage.GetComponent<Image>().sprite = layersprites[GameData.layer - 1].spriteData[0].sprites[2];
                break;
            case "gold":
                //��ҵ�ͼ�껹û��
                itemImage.GetComponent<Image>().sprite = null;
                break;
        }
        /*
        if (!gridShop.isInfinite) {
            shopStat.text = "������" + itemGiveOutNum + "��" + Hanize(itemGiveOut) +
                "�������" + itemExchangeForNum + "��" + Hanize(itemExchangeFor);
        } else {
            isThisShopInfinite = true;
            shopStat.text = "������" + itemGiveOutNum + "��" + Hanize(itemGiveOut) +
                "�������" + itemExchangeForNum + "��" + Hanize(itemExchangeFor) + "���������޵�";
        }*/
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
            case "B1":
                return "СѪƿ";
            case "B2":
                return "��Ѫƿ";
            case "B3":
                return "��Ѫƿ";
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
                //GameData.allGame_GoldGained += num;
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
            case "B1":
                for (int i = 0;i < num;i++) {
                    GameData.playerHp += GridBottle.healingPointsTable[0,GameData.layer - 1];
                }
                break;
            case "B2":
                for (int i = 0;i < num;i++) {
                    GameData.playerHp += GridBottle.healingPointsTable[1,GameData.layer - 1];
                }
                break;
            case "B3":
                for (int i = 0;i < num;i++) {
                    GameData.playerHp += GridBottle.healingPointsTable[2,GameData.layer - 1];
                }
                break;

        }
    }
    public void AffirmTrade() {
        if (IsEnough(itemExchangeFor,itemExchangeForNum)) {
            //������
            AddToInventory(itemExchangeFor,-itemExchangeForNum);
            AddToInventory(itemGiveOut,itemGiveOutNum);
            if(!isThisShopInfinite) {
                gameManager.GetComponent<GameManager>().ClearGridInMap(mapX,mapY);
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
