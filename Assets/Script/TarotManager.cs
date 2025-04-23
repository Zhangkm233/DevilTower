using System;
using UnityEngine;

public class TarotManager : MonoBehaviour
{
    public TarotsDataObject tarotsDataObject;

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
        Debug.Log("TarotManager³¢ÊÔUnlockTarot:" + index);
        GameData.tarotUnlock[index] = true;
    }
    public void UnlockTarot(string tarot) {
        Debug.Log("TarotManager³¢ÊÔUnlockTarot:" + tarot + TarotToNum(tarot));
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

    public int TarotToNum(string tarot) {
        for(int i = 0;i < tarotsDataObject.tarotsData.Count;i++) {
            if (tarotsDataObject.tarotsData[i].cardName == tarot) {
                return i;
            }
        }
        Debug.LogWarning("TarotManager: TarotToNum: " + tarot + " not found");
        return -99;
    }
    public string NumToTarot(int num) {
        return tarotsDataObject.tarotsData[num].cardName;
    }
}
