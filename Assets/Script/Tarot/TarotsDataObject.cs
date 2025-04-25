using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TarotsDataObject", menuName = "Scriptable Objects/TarotsDataObject")]

public class TarotsDataObject : ScriptableObject
{
    [System.Serializable]
    public class TarotData{
        public string cardName;
        public string description;
        public Sprite sprite;
        public string chineseName;
        //还没想好卡牌效果要怎么实现，以俟主程
    }
    
    public List<TarotData> tarotsData = new List<TarotData>();

    
}
