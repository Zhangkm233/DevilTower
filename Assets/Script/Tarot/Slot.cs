using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //private ParticleSystem particle;

    public void OnCardPutIn(){
        //particle.Play();
    }

    public void ExpandHalo(){
        GameObject halo = transform.Find("Halo").gameObject;
        StartCoroutine(EnxpandHaloCoroutine(halo, 1f));
    }

    IEnumerator EnxpandHaloCoroutine(GameObject halo, float duration){
        SpriteRenderer spriteRenderer = halo.GetComponent<SpriteRenderer>();

        float elapsed = 0;
        while (elapsed < duration) {

            halo.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, elapsed/duration);
            spriteRenderer.color = Color.Lerp(Color.white, new Color(1,1,1,0), elapsed/duration);

            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //particle = GetComponent<ParticleSystem>();
        GameObject halo = transform.Find("Halo").gameObject;
        halo.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
