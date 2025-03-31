using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotAnimHandler : MonoBehaviour
{
    public static TarotAnimHandler instance;
    public TarotsDataObject tarotsDataObject;

    public float animationDuration = 0.13f;

    public List<Slot> slots = new List<Slot>();
    public Slot targetSlot;
    //public TarotCard selectedCard;

    void Awake()
    {
        instance = this;
        targetSlot = slots[0];
    }

    public void selectTarot(TarotCard card){
        //更新选择的卡牌数据为这张
        //检查是否有空余
        int i = -1;
        for (i = 0;i < GameData.tarotEquip.Length;i++) {
            if (GameData.tarotEquip[i] == -1) {
                targetSlot = slots[i];
                Debug.Log("更新targetSlot");
                break;
            }
            if( i == GameData.tarotEquip.Length - 1) {
                Debug.Log("slot已满");
                return;
            }
        }
        //Gamedata. ...
        //进行动画效果
        /*
        TarotCard LastCard = null;
        if (selectedCard != null) {
            selectedCard.MoveOutSlot();
            LastCard = selectedCard;
            selectedCard = null;
            GameData.tarotEquip = 0;
        }
        if (LastCard != card) {
            selectedCard = card;
            GameData.tarotEquip = card.cardIndex;
        }*/
        GameData.tarotEquip[i] = card.cardIndex;
        card.isEquiping = true;
        card.MoveIntoSlot(targetSlot);
    }
    public void UnEquipTarot(TarotCard card) {
        for (int i = 0;i < GameData.tarotEquip.Length;i++) {
            if (GameData.tarotEquip[i] == card.cardIndex) {
                card.MoveOutSlot();
                card.isEquiping = false;
                GameData.tarotEquip[i] = -1;
                return;
            }
        }
        card.isEquiping=false;
    }

    public void ShakeAll(){
        StartCoroutine(ShakeAllCoroutine(0.2f));
    }

    IEnumerator ShakeAllCoroutine(float duration){
        float elapsed = 0;
        while (elapsed < duration) {
            transform.position = new Vector3(Mathf.Sin(100 * elapsed + 20), Mathf.Sin(100 * elapsed), 0) * 0.03f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = Vector3.zero;
    }

 
}
