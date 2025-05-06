using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndHandler : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject currentImage;
    public GameObject lastImage;
    public int spriteIndex = 0;
    public float fadeDuration = 2f;
    public bool isAutoPlay = true;
    public void Start() {
        spriteIndex = 0;
        currentImage.GetComponent<Image>().sprite = sprites[0];
        StartCoroutine(AutoPlay());
    }
    public void nextSprite() {
        if(!isAutoPlay || spriteIndex >= sprites.Length - 1) SceneManager.LoadScene("MainMenu");
        StopAllCoroutines();
        if(spriteIndex >= sprites.Length - 1) {
            isAutoPlay = false;
        } else {
            spriteIndex++;
            try {
                currentImage.GetComponent<Image>().sprite = sprites[spriteIndex];
                lastImage.GetComponent<Image>().sprite = sprites[spriteIndex];
                currentImage.GetComponent<Image>().color = new Color(1,1,1,1f);
                StartCoroutine(AutoPlay());
            } catch {
                Debug.Log("No more sprites");
            }
        }
    }

    public IEnumerator AutoPlay() {
        isAutoPlay = true;
        while (spriteIndex < sprites.Length) {
            lastImage.GetComponent<Image>().sprite = sprites[spriteIndex];
            currentImage.GetComponent<Image>().sprite = sprites[spriteIndex + 1];
            StartCoroutine(Fade());
            yield return new WaitForSeconds(5f);
        }
        isAutoPlay = false;
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
