using UnityEngine;

public class GridShop : Grid
{
    public static readonly string[,] shopItemStat = new string[6,2] {
            {"gold","B2"},
            {"gold","B3"},
            {"gold","key1"},
            {"gold","key2"},
            {"gold","key2"},
            {"key1","gold"},
        }; 
    public static readonly int[,] shopItemNum = new int[6,2] {
            {5,1},
            {20,1},
            {10,1},
            {30,1},
            {50,1},
            {1,100},
        };
    public bool isInfinite;
    public string itemGiveOut;
    public int itemGiveOutNum;
    public string itemExchangeFor;
    public int itemExchangeForNum;
    public GridShop(int stat) {
        this.type = GridType.SHOP;
        this.stat = stat;
        if(stat != 6) {
            isInfinite = false;
        } else {
            isInfinite = true;
        }
        //ÊÛ³ö
        itemGiveOut = shopItemStat[stat - 1,1];
        itemGiveOutNum = shopItemNum[stat - 1,1];
        //Ö§¸¶
        itemExchangeFor = shopItemStat[stat - 1,0];
        itemExchangeForNum = shopItemNum[stat - 1,0];
    }
}
