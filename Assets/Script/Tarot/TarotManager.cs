using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotManager : MonoBehaviour
{
    public static TarotManager instance;
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
        //更新选择的卡牌数据为这张
        //Gamedata. ...

        //进行动画效果
        if(selectedCard != null){
            StartCoroutine(MoveCardOutSlot(selectedCard, animationDuration));
        }
        selectedCard = card;
        StartCoroutine(MoveCardIntoSlot(targetSlot.transform, card, animationDuration));
    }

    IEnumerator MoveCardIntoSlot(Transform slot, TarotCard card, float duration){
        Vector3 startPos = card.transform.position;
        Vector3 targetPos = slot.position;

        float elapsed = 0;

        card.playParticle();
        while (elapsed < duration) {
            card.transform.position = Vector3.Lerp(startPos, targetPos, elapsed/duration);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        card.stopParticle();
        card.OnCardPutIn();
        targetSlot.OnCardPutIn();
        StartCoroutine(ShakeAllItems(0.2f));
    }

    IEnumerator MoveCardOutSlot(TarotCard card, float duration){
        Vector3 startPos = card.transform.position;
        Vector3 targetPos = card.originPos;
        
        float elapsed = 0;

        card.playParticle();
        while (elapsed < duration) {
            card.transform.position = Vector3.Lerp(startPos, targetPos, elapsed/duration);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        card.stopParticle();
    }

    IEnumerator ShakeAllItems(float duration){
        float elapsed = 0;
        while (elapsed < duration) {
            transform.position = new Vector3(Mathf.Sin(100 * elapsed + 20), Mathf.Sin(100 * elapsed), 0) * 0.03f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = Vector3.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
