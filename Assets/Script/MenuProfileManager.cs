using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuProfileManager : MonoBehaviour
{
    public int layerIndex = 0;
    public int monsterIndex = 0;
    public int monsterCount = 0;
    private string[] layerNames = {
        "魔女隐居之地",
        "倒吊人的阵枢之座",
        "月亮教会",
        "狂猎的驻扎地",
        "灵魂坟场",
        "征服者广场",
    };
    public TMP_Text layerText;
    public MonsterDataObject monsterDataObject;
    public SpriteScriptObject[] layerSpritess;
    public GameObject monsterProfile;
    public Text monsDescribe1;
    public Text monsDescribe2;
    public Text monsDescribe3;
    public void Awake() {
        layerText.text = "第" + (layerIndex + 1) + "层 " + layerNames[layerIndex];
        ReadMonster();
        UpdateMonsterText();
        UpdateMonsterDescribe();
    }

    public void NextLayer() {
        layerIndex++;
        if (layerIndex >= layerNames.Length) {
            layerIndex = 0;
        }
        UpdateLayerText();
        ReadMonster();
        monsterIndex = 0;
        UpdateMonsterText();
        UpdateMonsterDescribe();
    }

    public void LastLayer() {
        layerIndex--;
        if (layerIndex < 0) {
            layerIndex = layerNames.Length - 1;
        }
        UpdateLayerText();
        ReadMonster();
        monsterIndex = 0;
        UpdateMonsterText();
        UpdateMonsterDescribe();
    }

    public void NextMonster() {
        monsterIndex++;
        if (monsterIndex >= monsterCount) {
            monsterIndex = 0;
        }
        UpdateMonsterText();
        UpdateMonsterDescribe();
    }
    public void LastMonster() {
        monsterIndex--;
        if (monsterIndex < 0) {
            monsterIndex = monsterCount - 1;
        }
        UpdateMonsterText();
        UpdateMonsterDescribe();
    }

    public void UpdateLayerText() {
        layerText.text = "第" + (layerIndex + 1) + "层 " + layerNames[layerIndex];
    }

    public void UpdateMonsterText() {
        monsterProfile.GetComponent<MonsterProfile>().UseData(monsterDataObject.monsterDataList[monsterIndex]);
    }
    public void UpdateMonsterDescribe() {
        string monsFilePath1 = Application.streamingAssetsPath + "/monsterDescribe1.txt";
        int index1 = layerIndex switch {
            0 => 0,
            1 => 2,
            2 => 4,
            3 => 8,
            4 => 13,
            5 => 17,
            _ => 0,
        };
        index1 += monsterIndex;
        string[] lines1 = System.IO.File.ReadAllLines(monsFilePath1);
        if (index1 < lines1.Length) {
            monsDescribe1.text = "\u00A0\u00A0\u00A0\u00A0" + lines1[index1];
        } else {
            monsDescribe1.text = "没有描述";
        }

        string monsFilePath2 = Application.streamingAssetsPath + "/monsterDescribe2.txt";
        int index2 = layerIndex switch
        {
            0 => 0,
            1 => 2,
            2 => 4,
            3 => 8,
            4 => 13,
            5 => 17,
            _ => 0,
        };
        index2 += monsterIndex;
        string[] lines2 = System.IO.File.ReadAllLines(monsFilePath2);
        if (index2 < lines2.Length)
        {
            monsDescribe2.text = "\u00A0\u00A0\u00A0\u00A0" + lines2[index2];
        }
        else
        {
            monsDescribe2.text = "没有描述";
        }

        string monsFilePath3 = Application.streamingAssetsPath + "/monsterDescribe3.txt";
        int index3 = layerIndex switch
        {
            0 => 0,
            1 => 2,
            2 => 4,
            3 => 8,
            4 => 13,
            5 => 17,
            _ => 0,
        };
        index3 += monsterIndex;
        string[] lines3 = System.IO.File.ReadAllLines(monsFilePath3);
        if (index3 < lines3.Length)
        {
            monsDescribe3.text = "\u00A0\u00A0\u00A0\u00A0" + lines3[index3];
        }
        else
        {
            monsDescribe3.text = "没有描述";
        }
    }


    public void ReadMonster() {
        string txtFilePath = Application.streamingAssetsPath + "/monster" + (layerIndex+1) + ".txt";
        string[] lines = System.IO.File.ReadAllLines(txtFilePath);
        monsterCount = lines.Length;
        for (int i = 0;i < lines.Length;i++) {
            string[] monsterStat = lines[i].Split(' ');
            monsterDataObject.monsterDataList[i].name = monsterStat[0];
            monsterDataObject.monsterDataList[i].atk = int.Parse(monsterStat[1]);
            monsterDataObject.monsterDataList[i].def = int.Parse(monsterStat[2]);
            monsterDataObject.monsterDataList[i].hp = int.Parse(monsterStat[3]);
            monsterDataObject.monsterDataList[i].gold = int.Parse(monsterStat[4]);
            monsterDataObject.monsterDataList[i].abilities = "";
            monsterDataObject.monsterDataList[i].sprite = layerSpritess[layerIndex].spriteData[3].sprites[i];
            int abilityLength = monsterStat.Length - 5;
            if (abilityLength > 0) {
                for (int j = 0;j < abilityLength;j++) {
                    switch (monsterStat[j + 5]) {
                        case "LOSTMIND":
                            monsterDataObject.monsterDataList[i].abilities += "魔心 ";
                            break;
                        case "CRACK":
                            monsterDataObject.monsterDataList[i].abilities += "碎裂 ";
                            break;
                        case "FIRMNESS":
                            monsterDataObject.monsterDataList[i].abilities += "坚定 ";
                            break;
                        case "STALK":
                            monsterDataObject.monsterDataList[i].abilities += "追猎 ";
                            break;
                        case "CORRPUTIONONE":
                            monsterDataObject.monsterDataList[i].abilities += "腐蚀I ";
                            break;
                        case "CORRPUTIONTWO":
                            monsterDataObject.monsterDataList[i].abilities += "腐蚀II ";
                            break;
                        case "CORRPUTIONTHREE":
                            monsterDataObject.monsterDataList[i].abilities += "腐蚀III ";
                            break;
                        case "BOSS":
                            monsterDataObject.monsterDataList[i].abilities += "头目 ";
                            break;
                    }
                }
            }
        }
        if (monsterDataObject.monsterDataList.Count > lines.Length) {
            for (int j = lines.Length;j < monsterDataObject.monsterDataList.Count;j++) {
                monsterDataObject.monsterDataList[j].name = null;
                //monsterDataObject.monsterDataList[j].sprite = questionMarks[GameData.layer - 1];
                monsterDataObject.monsterDataList[j].atk = 0;
                monsterDataObject.monsterDataList[j].def = 0;
                monsterDataObject.monsterDataList[j].hp = 0;
                monsterDataObject.monsterDataList[j].gold = 0;
                monsterDataObject.monsterDataList[j].abilities = "";
            }
        }
    }
}
