using UnityEngine;

public class GridShop : Grid
{
    public static readonly string[,] shopItemStat = new string[4,2] {
            {"gold","key1"},
            {"gold","key2"},
            {"gold","key3"},
            {"gold","key2"},
        }; 
    public static readonly int[,] shopItemNum = new int[4,2] {
            {15,3},
            {15,1},
            {50,1},
            {30,1},
        };
    public string itemGiveOut;
    public int itemGiveOutNum;
    public string itemExchangeFor;
    public int itemExchangeForNum;
    public GridShop(int stat) {
        this.type = GridType.SHOP;
        this.stat = stat;
        //ÊÛ³ö
        itemGiveOut = shopItemStat[stat,1];
        itemGiveOutNum = shopItemNum[stat,1];
        //Ö§¸¶
        itemExchangeFor = shopItemStat[stat,0];
        itemExchangeForNum = shopItemNum[stat,0];
    }
}
