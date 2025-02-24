using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataObject", menuName = "Scriptable Objects/MonsterDataObject")]
public class MonsterDataObject : ScriptableObject
{
    [System.Serializable]
    public class MonsterData
    {
        public string name;
        public int atk;
        public int def;
        public int hp;
        public int gold;
        public ability[] abilityType;
        public Sprite sprite;
    }

    public enum ability
    {
        LOSTMIND,
        CRACK,
        FIRMNESS,
        STALK,
        CORRPUTION,
        BOSS,
    }

    public List<MonsterData> monsterDataList = new List<MonsterData>();
}
