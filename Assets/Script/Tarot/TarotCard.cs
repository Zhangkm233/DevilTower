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
    public GameObject audioManager;
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
            this.GetComponent<SpriteRenderer>().color = new Color(0.2f,0.2f,0.2f,1f);
        } else {
            this.GetComponent<SpriteRenderer>().sprite = tarotsDataObject.tarotsData[cardIndex].sprite;
            this.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        }
        Material material = GetComponent<SpriteRenderer>().material;
        switch (cardIndex) {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                material.renderQueue = 3000;
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
                material.renderQueue = 3001;
                break;
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
                material.renderQueue = 3002;
                break;
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
            audioManager.GetComponent<AudioManager>().PlayDef();
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
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale * 1.2f;
        while (elapsed < 0.07f) {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed/0.1f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator scaleDown(){
        float elapsed = 0;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale / 1.2f;
        while (elapsed < 0.07f) {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed/0.1f);
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
        GetComponent<BoxCollider2D>().enabled = false;
        Vector3 startPos = transform.position;
        Vector3 targetPos = slot.transform.position;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = slot.transform.localScale * 1.6f;

        float elapsed = 0;
        float speed = 30f;
        float distance = Vector3.Distance(startPos, targetPos);

        GetComponent<BoxCollider2D>().enabled = false;

        while (elapsed < distance / speed) {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed/(distance/speed));
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed/(distance/speed));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.localScale = targetScale;

        GetComponent<BoxCollider2D>().enabled = true;

        OnCardPutIn(slot);
    }

    IEnumerator MoveOutSlotCoroutine(){
        GetComponent<BoxCollider2D>().enabled = false;
        Vector3 startPos = transform.position;
        Vector3 targetPos = originPos;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(1f, 1f, 1f);

        float elapsed = 0;
        float speed = 40f;
        float distance = Vector3.Distance(startPos, targetPos);

        while (elapsed < distance / speed) {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed/(distance/speed));
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed/(distance/speed));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.localScale = targetScale;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
