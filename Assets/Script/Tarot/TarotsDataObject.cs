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
        //��û��ÿ���Ч��Ҫ��ôʵ�֣���ٹ����
    }
    
    public List<TarotData> tarotsData = new List<TarotData>();

    
}
