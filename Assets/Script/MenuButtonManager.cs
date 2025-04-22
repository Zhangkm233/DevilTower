using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    public float fadeDuration = 0.5f; // 渐变持续时间
    //public GameObject blackPanel;
    public CanvasGroup fadeCanvasGroup;
    public GameObject saveChoosePanel;
    public void Start() {
        saveChoosePanel.SetActive(false);
    }
    public void StartGame() {
        Debug.Log("Start Game button clicked");
        MenuData.isContinueGame = false;
        MenuData.loadGameSlot = 0;
        StartCoroutine(FadeAndLoadScene("MainGame"));
    }
    public void ContinueGame() {
        Debug.Log("Continue Game button clicked");
        MenuData.isContinueGame = true;
        saveChoosePanel.SetActive(true);
        GameObject[] slots = GameObject.FindGameObjectsWithTag("saveSlotGameObject");
        foreach (GameObject slot in slots) {
            slot.GetComponent<SaveSlotManager>().UpdateSaveSlotData();
        }
    }
    public void ExitGame() {
        Debug.Log("Exit Game button clicked");
        StartCoroutine(FadeAndLoadScene("Exit"));
    }


    public IEnumerator FadeAndLoadScene(string sceneTitle) {
        // 渐暗效果
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f,1f,elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // 确保完全变黑
        if(sceneTitle == "Exit") {
            Application.Quit();
        } else {
            SceneManager.LoadScene(sceneTitle);
        }
    }
}
