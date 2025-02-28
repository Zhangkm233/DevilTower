using UnityEngine;
using System.IO;
using Unity.VisualScripting.FullSerializer;

public class GridMonster : Grid
{
    public int atk;
    public int def;
    public int hp;
    public int gold;
    //public ability[] abilityType;
    public string txtFilePath;
    public string name;
    public bool isLostmind;
    public bool isCrack;
    public bool isFirmness;
    public bool isStalk;
    public bool isCorruptionOne;
    public bool isCorruptionTwo;
    public bool isCorruptionThree;
    public bool isBoss;
    /*
    public enum ability {
        LOSTMIND = 1,
        CRACK = 2,
        FIRMNESS = 3,
        STALK = 4,
        CORRPUTION = 5,
        BOSS = 6,
    }*/
    public GridMonster(int stat){
        GridTypeToWord = "M";
        this.stat = stat;
        this.type = GridType.MONSTER;
        txtFilePath = "Assets/Resources/monster" + GameData.layer + ".txt";
        string[] lines = File.ReadAllLines(txtFilePath);
        string[] monsterStat = lines[stat-1].Split(' ');
        name = monsterStat[0];
        atk = int.Parse(monsterStat[1]);
        def = int.Parse(monsterStat[2]);
        hp = int.Parse(monsterStat[3]);
        gold = int.Parse(monsterStat[4]);

        if (monsterStat.Length >= 6) {
            GainAbility(monsterStat[5]);
        }
        if (monsterStat.Length >= 7) {
            GainAbility(monsterStat[6]);
        }
    }

    void GainAbility(string ability) {
        switch (ability) {
            case "LOSTMIND":
                isLostmind = true; break;
            case "CRACK":
                isCrack = true; break;
            case "FIRMNESS":
                isFirmness = true; break;
            case "STALK":
                isStalk = true; break;
            case "CORRUPTIONTWO":
                isCorruptionOne = true; break;
            case "CORRUPTIONONE":
                isCorruptionTwo = true; break;
            case "CORRUPTIONTHREE":
                isCorruptionThree = true; break;
            case "BOSS":
                isBoss = true; break;
            default:
                break;
        }
    }

    public void introduceSelf() {
        Debug.Log("name:" + name + " atk:" + atk + " def:" + def + " hp:" + hp + " gold:" + gold);
    }

    public override Grid AsEnter() {
        Debug.Log("Enter Monster");
        return this;
    }
}
