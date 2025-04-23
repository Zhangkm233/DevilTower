using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class SaveSlotManager : MonoBehaviour
{
    public int saveSlotIndex;
    public string filePath;
    public GameObject saveDescribe1;
    public GameObject menuManager;
    public TarotsDataObject tarotsDataObject;
    public void UpdateSaveSlotData() {
        filePath = Application.persistentDataPath + "/savefile" + saveSlotIndex + ".json";
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Load:" + filePath);
            if (saveSlotIndex == 0) {
                saveDescribe1.GetComponent<Text>().text = "自动存档:";
            } else {
                saveDescribe1.GetComponent<Text>().text = "存档" + saveSlotIndex + ":";
            }
            saveDescribe1.GetComponent<Text>().text += "\n当前层数：" + data.layer.ToString();
            saveDescribe1.GetComponent<Text>().text += "\n遭遇事件：" + data.eventEncounter.ToString();
            saveDescribe1.GetComponent<Text>().text += "\n当前数值：" + data.playerHp.ToString() + "/" + data.playerAtk.ToString() + "/" + data.playerDef.ToString();
            saveDescribe1.GetComponent<Text>().text += "\n锻造次数：" + data.forgeTime.ToString();
            saveDescribe1.GetComponent<Text>().text += "\n当前金币：" + data.gold.ToString();
            string temp = "";
            for (int i = 0;i<data.tarotEquip.Length;i++) {
                if (data.tarotEquip[i] != -1) {
                    temp += NumToTarot(data.tarotEquip[i]) + " ";
                }
            }
            if(temp != "") {
                saveDescribe1.GetComponent<Text>().text += "\n塔罗牌：" + temp;
            } 
            this.GetComponent<Button>().interactable = true;
        } else {
            Debug.Log("Save file not found in " + filePath);
            saveDescribe1.GetComponent<Text>().text = "存档" + saveSlotIndex + ":";
            saveDescribe1.GetComponent<Text>().text += "暂无存档";
            this.GetComponent<Button>().interactable = false;
        }
    }
    public void ToGame() {
        if (File.Exists(filePath) == false) return;
        MenuData.loadGameSlot = saveSlotIndex;
        StartCoroutine(menuManager.GetComponent<MenuButtonManager>().FadeAndLoadScene("MainGame"));
    }
    public string NumToTarot(int num) {
        return tarotsDataObject.tarotsData[num].cardName;
    }

}
