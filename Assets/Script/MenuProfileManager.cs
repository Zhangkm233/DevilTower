using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuProfileManager : MonoBehaviour
{
    public int layerIndex = 0;
    public int monsterIndex = 0;
    public int monsterCount = 0;
    private string[] layerNames = {
        "ħŮ����֮��",
        "�����˵�����֮��",
        "�����̻�",
        "���Ե�פ����",
        "���س�",
        "�����߹㳡",
    };
    public TMP_Text layerText;
    public MonsterDataObject monsterDataObject;
    public SpriteScriptObject[] layerSpritess;
    public GameObject monsterProfile;

    public void Awake() {
        layerText.text = "��" + (layerIndex + 1) + "�� " + layerNames[layerIndex];
        ReadMonster();
        UpdateMonsterText();
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
    }

    public void NextMonster() {
        monsterIndex++;
        if (monsterIndex >= monsterCount) {
            monsterIndex = 0;
        }
        UpdateMonsterText();
    }
    public void LastMonster() {
        monsterIndex--;
        if (monsterIndex < 0) {
            monsterIndex = monsterCount - 1;
        }
        UpdateMonsterText();
    }

    public void UpdateLayerText() {
        layerText.text = "��" + (layerIndex + 1) + "�� " + layerNames[layerIndex];
    }

    public void UpdateMonsterText() {
        monsterProfile.GetComponent<MonsterProfile>().UseData(monsterDataObject.monsterDataList[monsterIndex]);
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
                            monsterDataObject.monsterDataList[i].abilities += "ħ�� ";
                            break;
                        case "CRACK":
                            monsterDataObject.monsterDataList[i].abilities += "���� ";
                            break;
                        case "FIRMNESS":
                            monsterDataObject.monsterDataList[i].abilities += "�ᶨ ";
                            break;
                        case "STALK":
                            monsterDataObject.monsterDataList[i].abilities += "׷�� ";
                            break;
                        case "CORRPUTIONONE":
                            monsterDataObject.monsterDataList[i].abilities += "��ʴI ";
                            break;
                        case "CORRPUTIONTWO":
                            monsterDataObject.monsterDataList[i].abilities += "��ʴII ";
                            break;
                        case "CORRPUTIONTHREE":
                            monsterDataObject.monsterDataList[i].abilities += "��ʴIII ";
                            break;
                        case "BOSS":
                            monsterDataObject.monsterDataList[i].abilities += "ͷĿ ";
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
