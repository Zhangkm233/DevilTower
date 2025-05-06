using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using JetBrains.Annotations;

public class MenuMirrorManager : MonoBehaviour
{
    public Sprite[] sprites; // �洢������Ҫ�л���Sprite
    public float fadeDuration = 1.0f; // ��������ʱ��
    public int currentIndex = 0; // ��ǰ��ʾ��Sprite����
    public Image im; // im���
    public float delayBetweenImages = 5f;
    public float elapsedTime = 0f;

    void Start() {
        //return;
        //im = GetComponent<Image>();

        // ȷ����ʼ״̬��ȷ
        
    }

    public void startFade() {
        if (sprites.Length > 0) {
            im.sprite = sprites[currentIndex];
            Debug.Log("��ʼЭ��");

            StartCoroutine(FadeAndSwitch());
        }
    }

    IEnumerator FadeAndSwitch() {
        while (true) {
            // ������ǰSprite
            elapsedTime = 0f;
            Color originalColor = im.color;

            yield return new WaitForSeconds(delayBetweenImages);

            Debug.Log("����");
            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f,0f,elapsedTime / fadeDuration);
                im.color = new Color(originalColor.r,originalColor.g,originalColor.b,alpha);
                yield return null;
            }

            Debug.Log("�л�sprite");
            // ��ȫ͸�����л�����һ��Sprite
            currentIndex = (currentIndex + 1) % sprites.Length;
            im.sprite = sprites[currentIndex];

            // ������ɫ������alphaֵ��
            im.color = originalColor;

            // ������������ӵ���Ч������ѡ��
            yield return StartCoroutine(FadeIn(fadeDuration));

            //yield return new WaitForSeconds(delayBetweenImages);
        }
    }

    // ����Ч������ѡ��
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