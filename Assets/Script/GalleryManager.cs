using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
public class GalleryManager : MonoBehaviour
{
    public GameObject musicPanel;
    public GameObject tarotPanel;
    public GameObject MonsterPanel;
    public GameObject musicButton;
    public GameObject tarotButton;
    public GameObject MonsterButton;
    public GameObject backButton;
    public enum GalleryState
    {
        Music,
        Tarot,
        Monster,
    }
    public GalleryState currentState = GalleryState.Music;
    public void initialAllMusic() {
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("musicGameObject");
        foreach (GameObject musicObject in musicObjects) {
            musicObject.GetComponent<MusicButtonManager>().UpdateMusicData();
        }
    }

    public void ChangeState(int stateIndex) {
        switch (stateIndex) {
            case 0:
                ChangeState(GalleryState.Music);
                break;
            case 1:
                ChangeState(GalleryState.Tarot);
                break;
            case 2:
                ChangeState(GalleryState.Monster);
                break;
            default:
                Debug.LogError("Invalid state index: " + stateIndex);
                break;
        }
    }
    public void ChangeState(GalleryState state) {
        currentState = state;
        musicPanel.SetActive(state == GalleryState.Music);
        tarotPanel.SetActive(state == GalleryState.Tarot);
        MonsterPanel.SetActive(state == GalleryState.Monster);
        ChangeInteractableOfButtons();
    }

    public void ChangeInteractableOfButtons() {
        musicButton.GetComponent<Button>().interactable = (currentState != GalleryState.Music);
        tarotButton.GetComponent<Button>().interactable = (currentState != GalleryState.Tarot);
        MonsterButton.GetComponent<Button>().interactable = (currentState != GalleryState.Monster);
    }
    public void ResetAllButtonPos() {
        musicButton.GetComponent<MenuButtonAnimManager>().resetPos();
        tarotButton.GetComponent<MenuButtonAnimManager>().resetPos();
        MonsterButton.GetComponent<MenuButtonAnimManager>().resetPos();
        backButton.GetComponent<MenuButtonAnimManager>().resetPos();
    }
}
