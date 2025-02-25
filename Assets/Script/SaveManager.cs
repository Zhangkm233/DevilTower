using System.IO;
using UnityEngine;


public class PlayerData
{
    public int layer;

    public void ReadDataFromGame(){
        layer = GameData.layer;
    }

    public void WriteDataToGame(){
        GameData.layer = layer;
    }
}

public static class SaveManager
{

    private static string filePath = Application.persistentDataPath + "/savefile.json";

    public static void Save(){

        PlayerData data = new PlayerData();
        data.ReadDataFromGame();

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }


    public static void Load(){

        if(File.Exists(filePath)){
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            data.WriteDataToGame();

        }else{
            Debug.Log("Save file not found in " + filePath);
        }

    }
}
