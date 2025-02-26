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
}
