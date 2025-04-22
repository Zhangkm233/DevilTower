using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public enum ability
    {
        LOSTMIND,
        CRACK,
        FIRMNESS,
        STALK,
        CORRPUTION,
        BOSS,
    }
    public string name;
    public int atk;
    public int def;
    public int hp;
    public int gold;
    public string abilities;
    //public ability[] abilityType;
    public Sprite sprite;
    public string intro;
}


[CreateAssetMenu(fileName = "MonsterDataObject", menuName = "Scriptable Objects/MonsterDataObject")]
public class MonsterDataObject : ScriptableObject
{
    public List<MonsterData> monsterDataList = new List<MonsterData>();
}
