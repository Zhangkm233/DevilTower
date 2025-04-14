using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    public float fadeDuration = 0.5f; // �������ʱ��
    //public GameObject blackPanel;
    public CanvasGroup fadeCanvasGroup;
    public void StartGame() {
        Debug.Log("Start Game button clicked");
        StartCoroutine(FadeAndLoadScene("MainGame"));
    }
    public void ExitGame() {
        Debug.Log("Exit Game button clicked");
        StartCoroutine(FadeAndLoadScene("Exit"));
    }


    public IEnumerator FadeAndLoadScene(string sceneTitle) {
        // ����Ч��
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f,1f,elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // ȷ����ȫ���
        if(sceneTitle == "Exit") {
            Application.Quit();
        } else {
            SceneManager.LoadScene(sceneTitle);
        }
    }
}
