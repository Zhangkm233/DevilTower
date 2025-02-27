using System.Collections;
using UnityEngine;

public class TarotCard : MonoBehaviour
{
    public int cardIndex;

    public Vector3 originPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originPos = transform.position;

        TarotsDataObject tarots = TarotManager.instance.tarotsDataObject;
        GetComponent<SpriteRenderer>().sprite = tarots.tarotsData[cardIndex].sprite;

        var collider = GetComponent<BoxCollider2D>();
        var sprite = GetComponent<SpriteRenderer>();
        collider.size = sprite.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleClick(){
        Debug.Log("Click");
        TarotManager.instance.selectTarot(this);
    }

    public void HandlePointerEnter(){
        Debug.Log("Hover");
        StartCoroutine(scaleUp());
    }

    public void HandlePointerExit(){
        Debug.Log("Exit");
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

    public void OnCardPutIn(){
        StartCoroutine(HighlightCoroutine(0.5f));
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
}
