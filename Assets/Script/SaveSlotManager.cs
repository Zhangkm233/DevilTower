using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class SaveSlotManager : MonoBehaviour
{
    public int saveSlotIndex;
    public string filePath;
    public GameObject saveDescribe1;
    public GameObject menuManager;
    public void UpdateSaveSlotData() {
        filePath = Application.persistentDataPath + "/savefile" + saveSlotIndex + ".json";
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Load:" + filePath);
            if (saveSlotIndex == 0) {
                saveDescribe1.GetComponent<Text>().text = "�Զ��浵:";
            } else {
                saveDescribe1.GetComponent<Text>().text = "�浵" + saveSlotIndex + ":";
            }
            saveDescribe1.GetComponent<Text>().text += "\n������" + data.layer.ToString();
            saveDescribe1.GetComponent<Text>().text += "\n�����¼�����" + data.eventEncounter.ToString();
        } else {
            Debug.Log("Save file not found in " + filePath);
            saveDescribe1.GetComponent<Text>().text = "�浵" + saveSlotIndex + ":";
            saveDescribe1.GetComponent<Text>().text += "���޴浵";
        }
    }
    public void ToGame() {
        if (File.Exists(filePath) == false) return;
        MenuData.loadGameSlot = saveSlotIndex;
        StartCoroutine(menuManager.GetComponent<MenuButtonManager>().FadeAndLoadScene("MainGame"));
    }
}
