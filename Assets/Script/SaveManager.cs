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
    public bool[] tarotMissionUnlock;
    public bool hasEncounterBoss;
    public int tarotEquip;
    public int allGame_DevilsDefeated;
    public int allGame_Door1Opened;
    public int allGame_SoulDefeated;
    public int allGame_GoldGained;
    public int allGame_EventEncountered;
    public bool isEventUsed;
    public int defeatSHDPJ;
    public int defeatSHWQJ;
    public string lastMonsterName;
    public int continueDefeatStatrack;

    public void ReadForeverDataFromGame() {
        tarotUnlock = GameData.tarotUnlock;
        tarotMissionUnlock = GameData.tarotMissionUnlock;
        allGame_DevilsDefeated = GameData.allGame_DevilsDefeated;
        allGame_Door1Opened = GameData.allGame_Door1Opened;
        allGame_SoulDefeated = GameData.allGame_SoulDefeated;
        allGame_GoldGained = GameData.allGame_GoldGained;
        allGame_EventEncountered = GameData.allGame_EventEncountered;
    }
    public void WriteForeverDataToGame() {
        if (GameData.GetTarotCount(GameData.tarotUnlock) < GameData.GetTarotCount(tarotUnlock)) {
            GameData.tarotUnlock = tarotUnlock;
        }
        if (GameData.GetTarotCount(GameData.tarotMissionUnlock) < GameData.GetTarotCount(tarotMissionUnlock)) {
            GameData.tarotMissionUnlock = tarotMissionUnlock;
        }
        GameData.allGame_DevilsDefeated = Mathf.Max(allGame_DevilsDefeated,GameData.allGame_DevilsDefeated);
        GameData.allGame_Door1Opened = Mathf.Max(allGame_Door1Opened,GameData.allGame_Door1Opened);
        GameData.allGame_SoulDefeated = Mathf.Max(allGame_SoulDefeated,GameData.allGame_SoulDefeated);
        GameData.allGame_GoldGained = Mathf.Max(allGame_GoldGained,GameData.allGame_GoldGained);
        GameData.allGame_EventEncountered = Mathf.Max(allGame_EventEncountered,GameData.allGame_EventEncountered);
    }
    public void ReadDataFromGame(){
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
        hasEncounterBoss = GameData.hasEncounterBoss;
        tarotEquip = GameData.tarotEquip;
        isEventUsed = GameData.isEventUsed;
        defeatSHDPJ = GameData.defeatSHDPJ;
        defeatSHWQJ = GameData.defeatSHWQJ;
        lastMonsterName = GameData.lastMonsterName;
        continueDefeatStatrack = GameData.continueDefeatStatrack;
    }

    public void WriteDataToGame(){
        Debug.Log("WriteDataToGame");
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
        GameData.hasEncounterBoss = hasEncounterBoss;
        GameData.tarotEquip = tarotEquip;
        GameData.isEventUsed = isEventUsed;
        GameData.defeatSHWQJ = defeatSHWQJ;
        GameData.defeatSHDPJ = defeatSHDPJ;
        GameData.lastMonsterName = lastMonsterName;
        GameData.continueDefeatStatrack = continueDefeatStatrack;
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
        Debug.Log("Save:" + filePath);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath,json);
    }
    public static void LoadForeverData() {
        if (File.Exists(foreverFilePath)) {
            string json = File.ReadAllText(foreverFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Load:" + filePath);
            data.WriteForeverDataToGame();
        } else {
            Debug.Log("Save file not found in " + filePath);
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
