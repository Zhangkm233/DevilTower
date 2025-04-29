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
    public void Fight() {
        bossHpSlider.value = (float)bossHp / (float)bossFullHp;
        gameManager.GetComponent<GameManager>().UpdatePlayerOffset();
        if (GameData.IsTarotEquip(14)) {
            bossHp -= GameData.playerTotalAtk;
        } else {
            bossHp -= (GameData.playerTotalAtk - bossDef);
        }
        bossHpSlider.value = (float)bossHp / (float)bossFullHp;

        if (bossHp < 0) {
            bossHp = 0;
        }
        int damage = (bossAtk - GameData.playerTotalDef);
        GameData.playerHp -= damage;
        gameManager.GetComponent<UIManager>().PopNumber(damage,Color.red);
    }
}
