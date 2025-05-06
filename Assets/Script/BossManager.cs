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
    public void Fight() {
        bossHpSlider.value = (float)bossHp / (float)bossFullHp;
        gameManager.GetComponent<GameManager>().UpdatePlayerOffset();
        if (GameData.IsTarotEquip(14)) {
            bossHp -= GameData.playerTotalAtk;
        } else {
            bossHp -= (GameData.playerTotalAtk - bossDef);
        }
        bossHpSlider.value = (float)bossHp / (float)bossFullHp;
        bossObject.GetComponent<Animator>().Play("bossHurt");
        if (bossHp <= 0) {
            bossHp = 0;
            //结束战斗
            for(int i = 0; i < GameData.tarotEquip.Length;i++) {
                if (GameData.tarotEquip[i] != -1 || GameData.tarotEquip[i] != 0) {
                    gameManager.GetComponent<UIManager>().FadeAndLoadScene("GoodEnd");
                    return;
                }
            }
            gameManager.GetComponent<UIManager>().FadeAndLoadScene("TrueEnd");
        }
        int damage = (bossAtk - GameData.playerTotalDef);
        GameData.playerHp -= damage;
        gameManager.GetComponent<UIManager>().PopNumber(damage,Color.red);
        if (GameData.playerHp <= 0) {
            GameData.playerHp = 0;
            //结束战斗
            gameManager.GetComponent<UIManager>().FadeAndLoadScene("BadEnd");
        }
    }
}
