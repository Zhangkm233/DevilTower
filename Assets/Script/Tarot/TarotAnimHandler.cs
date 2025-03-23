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
    public TarotCard selectedCard;

    void Awake()
    {
        instance = this;
        targetSlot = slots[0];
    }

    public void selectTarot(TarotCard card){
        //����ѡ��Ŀ�������Ϊ����
        //Gamedata. ...
        //���ж���Ч��
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
        }
        card.MoveIntoSlot(targetSlot);
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
