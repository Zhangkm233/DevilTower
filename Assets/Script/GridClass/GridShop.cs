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
            {30,3},
            {30,1},
            {80,1},
            {40,1},
        };
    public string itemGiveOut;
    public int itemGiveOutNum;
    public string itemExchangeFor;
    public int itemExchangeForNum;
    public GridShop(int stat) {
        this.type = GridType.SHOP;
        this.stat = stat;
        itemGiveOut = shopItemStat[stat,0];
        itemGiveOutNum = shopItemNum[stat,0];
        itemExchangeFor = shopItemStat[stat,1];
        itemExchangeForNum = shopItemNum[stat,1];
    }
}
