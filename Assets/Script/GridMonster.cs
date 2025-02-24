using UnityEngine;
using System.IO;
using Unity.VisualScripting.FullSerializer;

public class GridMonster : Grid
{
    public int atk;
    public int def;
    public int hp;
    public int gold;
    public ability[] abilityType;
    public string txtFilePath;
    public string name;
    public enum ability {
        /*
        lostmind 魔心 这个怪物如果已经到达最左侧的格子，就会前进到最右侧的格子 
        crack 碎裂 这个怪物会摧毁自己经过的宝石，药水和钥匙，而不是将它们扔到身后
        firmness 坚定 这个怪物不会移动
        stalk 追猎 从这个怪物进入前排起，每三个回合会进行一次攻击并对玩家造成伤害
        corrpution 腐蚀X 在开战前使玩家的血量降低X*10%
        boss 头目 被击败后出现可以离开区域的传送门
         */
        LOSTMIND,
        CRACK,
        FIRMNESS,
        STALK,
        CORRPUTION,
        BOSS,
    }
    public GridMonster(int stat){
        this.type = GridType.MONSTER;
        txtFilePath = "Assets/Resources/monster" + GameData.layer + ".txt";
        string[] lines = File.ReadAllLines(txtFilePath);
        string[] monsterStat = lines[stat-1].Split(' ');
        name = monsterStat[0];
        atk = int.Parse(monsterStat[1]);
        def = int.Parse(monsterStat[2]);
        hp = int.Parse(monsterStat[3]);
        gold = int.Parse(monsterStat[4]);

        if (monsterStat.Length > 5) {
            abilityType = new ability[monsterStat.Length - 5];
            for (int i = 5;i < monsterStat.Length;i++) {
                abilityType[i - 5] = (ability)System.Enum.Parse(typeof(ability),monsterStat[i]);
            }
        }

        Debug.Log("name:"+ name + " atk:" + atk + " def:" + def + " hp:" + hp + " gold:" + gold);
        if (abilityType != null) {
            for (int i = 0;i < abilityType.Length;i++) {
                Debug.Log("ability:" + abilityType[i]);
            }
        }
    }

    public override Grid AsEnter() {
        Debug.Log("Enter Monster");
        return this;
    }
}
