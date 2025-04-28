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
    public static int gridHeight = 4; // ��ͼ�߶�
    public static Grid[,] map = new Grid[gridWidth,gridHeight];

    public static int[] tarotLastEquip = { -1,-1,-1,-1,-1 }; //�ϴ�װ����������
    public static int[] tarotEquip = {-1,-1,-1,-1,-1};
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
}
