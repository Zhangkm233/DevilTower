using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerData
{
    public int layer;
    public int key1;
    public int key2;
    public int key3;
    public int gold;
    public int playerHp;
    public int playerDef;
    public int playerAtk;
    public int forgeTime;
    public int eventEncounter;
    public int GridWidth;
    public int GridHeight;
    public GridWrapper gridWrapper;
    public bool[] tarotUnlock;
    public int[] tarotEquip; 
    public float sfxVolume;
    public float bgmVolume;

    public bool isDeathBuff = false;
    public string lastMonsterName;
    public int popeBuffTime;
    public int atkOffsetInt;
    public int defOffsetInt;

    public void ReadForeverDataFromGame() {
        sfxVolume = GameData.sfxVolume;
        bgmVolume = GameData.bgmVolume;
    }
    public void WriteForeverDataToGame() {
        GameData.sfxVolume = sfxVolume;
        GameData.bgmVolume = bgmVolume;
    }
    public void ReadDataFromGame() {
        tarotUnlock = GameData.tarotUnlock;
        layer = GameData.layer;
        key1 = GameData.key1;
        key2 = GameData.key2;
        key3 = GameData.key3;
        gold = GameData.gold;
        playerHp = GameData.playerHp;
        playerAtk = GameData.playerAtk;
        playerDef = GameData.playerDef;
        forgeTime = GameData.forgeTime;
        eventEncounter = GameData.eventEncounter;
        GridWidth = GameData.gridWidth;
        GridHeight = GameData.gridHeight;
        gridWrapper = new GridWrapper(GridWidth,GridHeight);
        gridWrapper.SetGrid(GameData.map,GridWidth,GridHeight);
        tarotEquip = GameData.tarotEquip;
        isDeathBuff = GameData.isDeathBuff;
        lastMonsterName = GameData.lastMonsterName;
        popeBuffTime = GameData.popeBuffTime;
        atkOffsetInt = GameData.atkOffsetInt;
        defOffsetInt = GameData.defOffsetInt;
    }

    public void WriteDataToGame(){
        Debug.Log("WriteDataToGame");
        GameData.tarotUnlock = tarotUnlock;
        GameData.layer = layer;
        GameData.key1 = key1;
        GameData.key2 = key2;
        GameData.key3 = key3;
        GameData.gold = gold;
        GameData.playerHp = playerHp;
        GameData.playerAtk = playerAtk;
        GameData.playerDef = playerDef;
        GameData.forgeTime = forgeTime;
        GameData.eventEncounter = eventEncounter;
        GameData.gridWidth = GridWidth;
        GameData.gridHeight = GridHeight;
        GameData.map = new Grid[GameData.gridWidth,GameData.gridHeight];
        GameData.map = gridWrapper.GetGrid(GridWidth,GridHeight);
        GameData.tarotEquip = tarotEquip;
        GameData.isDeathBuff = isDeathBuff;
        GameData.lastMonsterName = lastMonsterName;
        GameData.popeBuffTime = popeBuffTime;
        GameData.atkOffsetInt = atkOffsetInt;
        GameData.defOffsetInt = defOffsetInt;
    }
}

public static class SaveManager
{
    private static string foreverFilePath = Application.persistentDataPath + "/saveForeverFile.json";
    private static string filePath = Application.persistentDataPath + "/savefile.json";
    public static void Delete(int saveIndex) {
        filePath = Application.persistentDataPath + "/savefile" + saveIndex + ".json";
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }
    }
    public static void Delete() {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }
    }
    public static void Save(int saveIndex) {
        filePath = Application.persistentDataPath + "/savefile" + saveIndex + ".json";
        Save();
    }
    public static void Load(int saveIndex) {
        filePath = Application.persistentDataPath + "/savefile" + saveIndex + ".json";
        Load();
    }
    public static void SaveForeverData() {
        PlayerData data = new PlayerData();
        data.ReadForeverDataFromGame();
        Debug.Log("Save:" + foreverFilePath);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(foreverFilePath,json);
    }
    public static void LoadForeverData() {
        if (File.Exists(foreverFilePath)) {
            string json = File.ReadAllText(foreverFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Load:" + foreverFilePath);
            data.WriteForeverDataToGame();
        } else {
            Debug.LogWarning("Save file not found in " + foreverFilePath);
        }
    }

    public static void Save(){

        PlayerData data = new PlayerData();
        data.ReadDataFromGame();
        Debug.Log("Save:" + filePath);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }


    public static void Load(){

        if(File.Exists(filePath)){
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Load:" + filePath);
            data.WriteDataToGame();

        }else{
            Debug.Log("Save file not found in " + filePath);
        }

    }
}
