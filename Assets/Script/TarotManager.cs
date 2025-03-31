using UnityEngine;

public class TarotManager : MonoBehaviour
{
    public TarotsDataObject tarotsDataObject;

    public void EffectsWhenEquipTarot(int tarotIndex) {
        switch (tarotIndex) {
            //根据不同的塔罗牌执行“选择时”的效果，是在结束塔罗牌选择界面的按钮上挂载的方法
        }
    }
    //任务
    /*
    public void UnlockMission(string tarot) {
        GameData.tarotMissionUnlock[TarotToNum(tarot)] = true;
    }
    public void UnlockMission(int index) {
        GameData.tarotMissionUnlock[index] = true;
    }
    public void UnlockAllMission() {
        for (int i = 0;i < GameData.tarotMissionUnlock.Length;i++) {
            GameData.tarotMissionUnlock[i] = true;
        }
    }
    public bool IsMissionUnlock(int index) {
        return GameData.tarotMissionUnlock[index];
    }
    public bool IsMissionUnlock(string tarot) {
        return GameData.tarotMissionUnlock[TarotToNum(tarot)];
    }
    */
    //塔罗牌
    public void UnlockTarot(int[] index) {
        for (int i = 0;i < index.Length;i++) {
            GameData.tarotUnlock[index[i]] = true;
        }
    }
    public void UnlockTarot(string[] tarot) {
        for (int i = 0;i < tarot.Length;i++) {
            GameData.tarotUnlock[TarotToNum(tarot[i])] = true;
        }
    }
    public void UnlockTarot(int index) {
        GameData.tarotUnlock[index] = true;
    }
    public void UnlockTarot(string tarot) {
        GameData.tarotUnlock[TarotToNum(tarot)] = true;
    }
    public void LockTarot(int index) {
        GameData.tarotUnlock[index] = false;
    }
    public void LockTarot(string tarot) {
        GameData.tarotUnlock[TarotToNum(tarot)] = false;
    }
    public bool IsTarotUnlock(int index) {
        return GameData.tarotUnlock[index];
    }
    public bool IsTarotUnlock(string tarot) {
        return GameData.tarotUnlock[TarotToNum(tarot)];
    }
    public void ResetTarot() {
        for (int i = 0;i < GameData.tarotUnlock.Length;i++) {
            GameData.tarotUnlock[i] = false;
        }
    }
    public void UnlockAllTarot() {
        for (int i = 0;i < GameData.tarotUnlock.Length;i++) {
            GameData.tarotUnlock[i] = true;
        }
    }

    /// <summary>
    /// PlayerPrefs会存到注册表，所以暂时先不用
    /// </summary>
    /*
    public void SaveTarotData() {
        PlayerPrefs.SetInt("tarotUnlockCount",GetTarotCount());
        for (int i = 0;i < GameData.tarotUnlock.Length;i++) {
            PlayerPrefs.SetInt("tarotUnlock" + i,GameData.tarotUnlock[i] ? 1 : 0);
        }
    }
    public void LoadTarotData() {
        ResetTarot();
        int count = PlayerPrefs.GetInt("tarotUnlockCount");
        for (int i = 0;i < count;i++) {
            GameData.tarotUnlock[i] = PlayerPrefs.GetInt("tarotUnlock" + i) == 1;
        }
    }
    public void ClearTarotData() {
        PlayerPrefs.DeleteAll();
    }
    */

    public int TarotToNum(string tarot) {
        for(int i = 0;i < tarotsDataObject.tarotsData.Count;i++) {
            if (tarotsDataObject.tarotsData[i].cardName == tarot) {
                return i;
            }
        }
        return -1;
    }
    public string NumToTarot(int num) {
        return tarotsDataObject.tarotsData[num].cardName;
    }
}
