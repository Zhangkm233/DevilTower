using UnityEngine;
using UnityEngine.UI;

public class MusicButtonManager : MonoBehaviour
{
    public int musicIndex;
    public GameObject musicDescribeText;
    public GameObject audioManager;
    public string[] musicNames = new string[13] 
    {"bgm_1", "bgm_2", "bgm_3", "bgm_4", "bgm_5", "bgm_6", "bgm_7", "bgm_8", "bgm_9", "bgm_10", "bgm_11", "bgm_12", "bgm_13"};
    public void UpdateMusicData(){
        musicDescribeText.GetComponent<Text>().text = musicNames[musicIndex] + " ";
        musicDescribeText.GetComponent<Text>().text += audioManager.GetComponent<AudioManager>().backGroundMusics[musicIndex].name + " ";
        musicDescribeText.GetComponent<Text>().text += (int)audioManager.GetComponent<AudioManager>().backGroundMusics[musicIndex].length + "s";
    }
    public void PlayMusic() {
        audioManager.GetComponent<AudioManager>().PlayBgm(musicIndex);
    }
}
