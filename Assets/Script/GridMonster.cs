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
        lostmind ħ�� �����������Ѿ����������ĸ��ӣ��ͻ�ǰ�������Ҳ�ĸ��� 
        crack ���� ��������ݻ��Լ������ı�ʯ��ҩˮ��Կ�ף������ǽ������ӵ����
        firmness �ᶨ ������ﲻ���ƶ�
        stalk ׷�� ������������ǰ����ÿ�����غϻ����һ�ι��������������˺�
        corrpution ��ʴX �ڿ�սǰʹ��ҵ�Ѫ������X*10%
        boss ͷĿ �����ܺ���ֿ����뿪����Ĵ�����
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
