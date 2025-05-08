using UnityEngine;

[System.Serializable]
public static class GameData
{
    public static int eventCount = 24; //�¼�����
    public static int eventEncounter = 0; //�������¼�����
    public static int layer = 1;
    public static int key1 = 0;
    public static int key2 = 0;
    public static int key3 = 0;
    public static int gold = 0;

    public static int playerHp = 100;
    public static int playerDef = 5;
    public static int playerAtk = 5;

    public static int forgeTime = 0;//��������������

    public static bool hasEncounterBoss = false;
    public static int gridWidth = 6; // ��ͼ���
    public static int gridHeight = 7; // ��ͼ�߶�
    public static Grid[,] map = new Grid[gridWidth,gridHeight];

    public static int[] tarotLastEquip = { -1,-1,-1,-1,-1 }; //�ϴ�װ����������
    public static int[] tarotEquip = { -1,-1,-1,-1,-1 };
    public static bool[] tarotUnlock = new bool[22];

    public static int saveSlotChoose = 0;

    public static float sfxVolume = 0.4f;
    public static float bgmVolume = 0f;

    public static string lastMonsterName = string.Empty;
    public static bool isDeathBuff = false;
    public static int popeBuffTime = 0;
    public static int atkOffsetInt = 0;
    public static int defOffsetInt = 0;
    public static int playerTotalAtk = 0;
    public static int playerTotalDef = 0;

    public static int[] npcEncounteredLayer12 = { -1,-1,-1,-1,-1,-1};
    public static int[] npcEncounteredLayer34 = { -1,-1,-1,-1,-1,-1};
    public static int[] npcEncounteredLayer56 = { -1,-1,-1,-1,-1,-1};
    public static int GetTarotCount(bool[] tarots) {
        int count = 0;
        for (int i = 0;i < tarots.Length;i++) {
            if (tarots[i]) {
                count++;
            }
        }
        return count;
    }
    public static bool IsTarotEquip(int tarotIndex) {
        for (int i = 0;i < tarotEquip.Length;i++) {
            if (tarotEquip[i] == tarotIndex) {
                return true;
            }
        }
        return false;
    }
    public static bool IsTarotLastEquip(int tarotIndex) {
        for (int i = 0;i < tarotLastEquip.Length;i++) {
            if (tarotLastEquip[i] == tarotIndex) {
                return true;
            }
        }
        return false;
    }

    public static int GetRandomNumberExclude(int min,int max,int exclude) {
        int randomNumber;
        do {
            randomNumber = Random.Range(min,max);
        } while (randomNumber == exclude);
        return randomNumber;
    }
    public static int GetRandomNumberExclude(int min,int max,int[] exclude) {
        int randomNumber;
        bool isExclude;
        do {
            randomNumber = Random.Range(min,max);
            isExclude = false;
            for (int i = 0;i < exclude.Length;i++) {
                if (randomNumber == exclude[i]) {
                    isExclude = true;
                    break;
                }
            }
        } while (isExclude);
        return randomNumber;
    }


    public static string Hanize(string str) {
        //����
        switch (str) {
            case "gold":
                return "ħ���ᾧ";
            case "key1":
                return "��ͭԿ��";
            case "key2":
                return "����Կ��";
            case "key3":
                return "�ƽ�Կ��";
            case "B1":
                return "СѪƿ";
            case "B2":
                return "��Ѫƿ";
            case "B3":
                return "��Ѫƿ";
            default:
                break;
        }
        return "?";
    }
}
