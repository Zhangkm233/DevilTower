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
    public GameObject galleyPanel;
    public GameObject optionPanel;
    public GameObject audioManager;
    public AudioSource bgmAudioSource;
    public GameObject[] mainMenuObjects;
    public GameObject standbyCanvas;
    public GameObject mirrorIlluObject;
    public GameObject worldObject;
    public bool isStandBy = false;
    public void Start() {
        if(MenuData.isFirstStart) {
            MenuData.isFirstStart = false;
            isStandBy = true;
            ShowStandBy();
        }
        //backToMenu();
        SaveManager.LoadForeverData();
        if (MenuData.isCompleteGame) {
            worldObject.SetActive(true);
        } else {
            worldObject.SetActive(false);
        }
        audioManager.GetComponent<AudioManager>().InitialVolume();
    }
    public void Update() {
        if(isStandBy) {
            if (Input.anyKeyDown) {
                isStandBy = false;
                backToMenu();
            }
        }
    }
    public void ShowStandBy() {
        standbyCanvas.SetActive(true);
        foreach (GameObject obj in mainMenuObjects) {
            obj.SetActive(false);
        }
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
        foreach (GameObject obj in mainMenuObjects) {
            obj.SetActive(false);
        }
    }
    public void backToMenu() {
        saveChoosePanel.SetActive(false);
        galleyPanel.SetActive(false);
        optionPanel.SetActive(false);
        standbyCanvas.SetActive(false);
        foreach (GameObject obj in mainMenuObjects) {
            obj.SetActive(true);
        }
        mirrorIlluObject.SetActive(true);
        mirrorIlluObject.GetComponent<MenuMirrorManager>().startFade();
    }
    public void ToGallery() {
        Debug.Log("Gallery button clicked");
        galleyPanel.SetActive(true);
        saveChoosePanel.SetActive(false);
        foreach (GameObject obj in mainMenuObjects) {
            obj.SetActive(false);
        }
        galleyPanel.GetComponent<GalleryManager>().initialAllMusic();
        galleyPanel.GetComponent<GalleryManager>().ChangeState(GalleryManager.GalleryState.Tarot);
        galleyPanel.GetComponent<GalleryManager>().ChangeInteractableOfButtons(); 
        //galleyPanel.GetComponent<GalleryManager>().ResetAllButtonPos();
    }
    public void ToOption() {
        Debug.Log("Option button clicked");
        optionPanel.SetActive(true);
        saveChoosePanel.SetActive(false);
        galleyPanel.SetActive(false);
        foreach (GameObject obj in mainMenuObjects) {
            obj.SetActive(false);
        }
    }

    public void ExitGame() {
        Debug.Log("Exit Game button clicked");
        StartCoroutine(FadeAndLoadScene("Exit"));
    }
    public void SaveForeverData() {
        SaveManager.SaveForeverData();
    }
    
    public void ClearAllData() {
        SaveManager.DeleteForeverData();
        for (int i = 0;i < 5;i++) {
            SaveManager.Delete(i);
        }
    }
    public IEnumerator FadeAndLoadScene(string sceneTitle) {
        // 渐暗效果
        float elapsedTime = 0f;
        float bgmVolume = bgmAudioSource.volume;
        while (elapsedTime < fadeDuration) {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f,1f,elapsedTime / fadeDuration);
            bgmAudioSource.volume = Mathf.Lerp(bgmVolume,0f,elapsedTime / fadeDuration);
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
