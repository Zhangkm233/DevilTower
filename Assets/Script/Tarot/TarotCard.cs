using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TarotCard : MonoBehaviour
{
    public int cardIndex;
    public bool isEquiping;
    public Vector3 originPos;
    public GameObject cardDescribe;
    public GameObject cardDescribeImage;
    public GameObject cardDescribeName;
    public GameObject cardDescribeText;
    public TarotsDataObject tarotsDataObject;
    public Sprite lockSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originPos = transform.position;

        TarotsDataObject tarots = TarotAnimHandler.instance.tarotsDataObject;
        GetComponent<SpriteRenderer>().sprite = tarots.tarotsData[cardIndex].sprite;
        //cardDescribe = GameObject.Find("CardDescribe");
        //cardDescribeImage = cardDescribe.transform.GetChild(0).gameObject;
        //cardDescribeName = cardDescribe.transform.GetChild(1).gameObject;
        //cardDescribeText = cardDescribe.transform.GetChild(2).gameObject;
        var collider = GetComponent<BoxCollider2D>();
        var sprite = GetComponent<SpriteRenderer>();
        collider.size = sprite.bounds.size;
        CheckIfThisUnlock();
    }

    public void CheckIfThisUnlock() {
        if(GameData.tarotUnlock[cardIndex] == false) {
            this.GetComponent<SpriteRenderer>().sprite = lockSprite;
        } else {
            this.GetComponent<SpriteRenderer>().sprite = tarotsDataObject.tarotsData[cardIndex].sprite;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleClick(){
        Debug.Log("Click");
        if (GameData.tarotUnlock[cardIndex] == false) {
            return;
        }
        if (isEquiping) {
            TarotAnimHandler.instance.UnEquipTarot(this);
            return;
        }
        if (!isEquiping) {
            TarotAnimHandler.instance.selectTarot(this);
            return;
        }
    }

    public void HandlePointerEnter(){
        Debug.Log("Hover");
        if (GameData.tarotUnlock[cardIndex] == false) {
            return;
        }
        cardDescribe.SetActive(true);
        cardDescribeImage.GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        cardDescribeName.GetComponent<Text>().text = tarotsDataObject.tarotsData[cardIndex].cardName;
        cardDescribeText.GetComponent<Text>().text = tarotsDataObject.tarotsData[cardIndex].description;
        StartCoroutine(scaleUp());
    }

    public void HandlePointerExit(){
        Debug.Log("Exit");
        if (GameData.tarotUnlock[cardIndex] == false) {
            return;
        }
        cardDescribe.SetActive(false);
        StartCoroutine(scaleDown());
    }

    IEnumerator scaleUp(){
        float elapsed = 0;
        while (elapsed < 0.07f) {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, elapsed/0.1f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator scaleDown(){
        float elapsed = 0;
        while (elapsed < 0.07f) {
            transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, elapsed/0.1f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void playParticle(){
        GetComponent<ParticleSystem>().Play();
    }

    public void stopParticle(){
        GetComponent<ParticleSystem>().Stop();
    }

    public void ClearParticle(){
        GetComponent<ParticleSystem>().Clear();
    }

    public void OnCardPutIn(Slot slot){
        StartCoroutine(HighlightCoroutine(0.5f));
        //playParticle();
        TarotAnimHandler.instance.ShakeAll();
        //slot.ExpandHalo();
    }

    public void MoveIntoSlot(Slot slot){
        StartCoroutine(MoveIntoSlotCoroutine(slot));
    }

    public void MoveOutSlot(){
        stopParticle();
        StartCoroutine(MoveOutSlotCoroutine());
    }

    IEnumerator HighlightCoroutine(float duration)

    {

        Material material = GetComponent<SpriteRenderer>().material;
        float _whiteAmount = 0f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            _whiteAmount = Mathf.Lerp(1, 0, elapsed / duration);
            material.SetFloat("_WhiteAmount", _whiteAmount);
            elapsed += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("_WhiteAmount", 0);
    }

    IEnumerator MoveIntoSlotCoroutine(Slot slot){
        Vector3 startPos = transform.position;
        Vector3 targetPos = slot.transform.position;

        float elapsed = 0;
        float speed = 30f;
        float distance = Vector3.Distance(startPos, targetPos);

        GetComponent<BoxCollider2D>().enabled = false;

        while (elapsed < distance / speed) {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed/(distance/speed));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        GetComponent<BoxCollider2D>().enabled = true;

        OnCardPutIn(slot);
    }

    IEnumerator MoveOutSlotCoroutine(){
        Vector3 startPos = transform.position;
        Vector3 targetPos = originPos;

        float elapsed = 0;
        float speed = 40f;
        float distance = Vector3.Distance(startPos, targetPos);

        while (elapsed < distance / speed) {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed/(distance/speed));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
}
