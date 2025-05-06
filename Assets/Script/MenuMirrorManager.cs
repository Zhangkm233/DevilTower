using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using JetBrains.Annotations;

public class MenuMirrorManager : MonoBehaviour
{
    public Sprite[] sprites; // 存储所有需要切换的Sprite
    public float fadeDuration = 1.0f; // 淡出持续时间
    public int currentIndex = 0; // 当前显示的Sprite索引
    public Image im; // im组件
    public float delayBetweenImages = 5f;
    public float elapsedTime = 0f;

    void Start() {
        //return;
        //im = GetComponent<Image>();

        // 确保初始状态正确
        
    }

    public void startFade() {
        if (sprites.Length > 0) {
            im.sprite = sprites[currentIndex];
            Debug.Log("开始协程");

            StartCoroutine(FadeAndSwitch());
        }
    }

    IEnumerator FadeAndSwitch() {
        while (true) {
            // 淡出当前Sprite
            elapsedTime = 0f;
            Color originalColor = im.color;

            yield return new WaitForSeconds(delayBetweenImages);

            Debug.Log("淡出");
            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f,0f,elapsedTime / fadeDuration);
                im.color = new Color(originalColor.r,originalColor.g,originalColor.b,alpha);
                yield return null;
            }

            Debug.Log("切换sprite");
            // 完全透明后切换到下一个Sprite
            currentIndex = (currentIndex + 1) % sprites.Length;
            im.sprite = sprites[currentIndex];

            // 重置颜色（包括alpha值）
            im.color = originalColor;

            // 可以在这里添加淡入效果（可选）
            yield return StartCoroutine(FadeIn(fadeDuration));

            //yield return new WaitForSeconds(delayBetweenImages);
        }
    }

    // 淡入效果（可选）
    IEnumerator FadeIn(float duration) {
        float elapsedTime = 0f;
        Color originalColor = im.color;
        im.color = new Color(originalColor.r,originalColor.g,originalColor.b,0);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f,1f,elapsedTime / duration);
            im.color = new Color(originalColor.r,originalColor.g,originalColor.b,alpha);
            yield return null;
        }

        im.color = originalColor;
    }
}