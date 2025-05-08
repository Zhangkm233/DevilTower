using UnityEngine;
using UnityEngine.UI;
using System;

public class BossManager : MonoBehaviour
{
    public int bossFullHp = 1000;
    public int bossHp = 1000;
    public int bossAtk = 10;
    public int bossDef = 0;
    public Slider bossHpSlider;
    public GameObject gameManager;
    public GameObject bossObject;
    public GameObject audioManager;
    public void Fight() {
        //需要添加一个如果双方都破不了防的判断
        audioManager.GetComponent<AudioManager>().PlayBattle();
        bossHpSlider.value = (float)bossHp / (float)bossFullHp;
        gameManager.GetComponent<GameManager>().UpdatePlayerOffset();
        //给你一拳
        if (GameData.IsTarotEquip(14)) {
            bossHp -= Math.Max(GameData.playerTotalAtk,0);
        } else {
            bossHp -= Math.Max((GameData.playerTotalAtk - bossDef),0);
        }
        bossHpSlider.value = (float)bossHp / (float)bossFullHp;
        bossObject.GetComponent<Animator>().Play("bossHurt");
        if (bossHp <= 0) {
            bossHp = 0;
            //结束战斗
            for(int i = 0; i < GameData.tarotEquip.Length;i++) {
                if (GameData.tarotEquip[i] != -1 || GameData.tarotEquip[i] != 0) {
                    StartCoroutine(gameManager.GetComponent<UIManager>().FadeAndLoadScene("GoodEnd"));
                    Debug.Log("good end");
                    return;
                }
            }
            StartCoroutine(gameManager.GetComponent<UIManager>().FadeAndLoadScene("TrueEnd"));
        }
        //给我一拳
        int damage = Math.Max((bossAtk - GameData.playerTotalDef),0);
        GameData.playerHp -= damage;
        gameManager.GetComponent<UIManager>().PopNumber(damage,Color.red);
        if (GameData.playerHp <= 0) {
            GameData.playerHp = 0;
            //结束战斗
            StartCoroutine(gameManager.GetComponent<UIManager>().FadeAndLoadScene("BadEnd"));
        }

        updateBossDataToUI();
    }

    public void updateBossDataToUI() {
        gameManager.GetComponent<UIManager>().gridName.text = "暴君";
        gameManager.GetComponent<UIManager>().gridStat.text = bossAtk + "/" + bossDef + "/" + bossHp;
        //为什么生效不了？
        //为什么
        //为什么
    }
}
