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
        tarotUnlock = GameData.tarotUnlock;
        tarotMissionUnlock = GameData.tarotMissionUnlock;
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
        GameData.tarotUnlock = tarotUnlock;
        GameData.tarotMissionUnlock = tarotMissionUnlock;
    }
}

public static class SaveManager
{

    private static string filePath = Application.persistentDataPath + "/savefile.json";

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
