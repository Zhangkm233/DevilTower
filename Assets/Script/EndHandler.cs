using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndHandler : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject currentImage;
    public GameObject lastImage;
    public int spriteIndex = 0;
    public float fadeDuration = 2f;
    public void Start() {
        spriteIndex = 0;
        currentImage.GetComponent<Image>().sprite = sprites[0];
        StartCoroutine(AutoPlay());
    }
    public void nextSprite() {
        StopAllCoroutines();
        spriteIndex++;
        currentImage.GetComponent<Image>().sprite = sprites[spriteIndex];
        lastImage.GetComponent<Image>().sprite = sprites[spriteIndex];
        currentImage.GetComponent<Image>().color = new Color(1,1,1,1f);
        StartCoroutine(AutoPlay());
    }

    public IEnumerator AutoPlay() {
        while (spriteIndex < sprites.Length) {
            lastImage.GetComponent<Image>().sprite = sprites[spriteIndex];
            currentImage.GetComponent<Image>().sprite = sprites[spriteIndex];
            StartCoroutine(Fade());
            yield return new WaitForSeconds(5f);
        }
    }
    public IEnumerator Fade() {
        Image image = currentImage.GetComponent<Image>();
        image.color = new Color(1,1,1,0);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            image.color = new Color(1f,1f,1f,elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(1,1,1,1f);

        spriteIndex++;
    }
}
