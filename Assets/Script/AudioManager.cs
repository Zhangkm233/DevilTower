using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource bgmSource;
    public AudioClip sfx_battle;
    public AudioClip sfx_pick;
    public AudioClip sfx_click;
    public AudioClip[] backGroundMusics;
    public GameObject bgmVolumeBar;
    public GameObject sfxVolumeBar;
    public void PlayAudioClip(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayBattle() {
        sfxSource.PlayOneShot(sfx_battle);
    }
    public void PlayPick() {
        sfxSource.PlayOneShot(sfx_pick);
    }
    public void PlayClick() {
        sfxSource.PlayOneShot(sfx_click);
    }
    public void PlayBgm(int index) {
        if (index < 0 || index >= backGroundMusics.Length) {
            Debug.LogError("Index out of range");
            return;
        }
        bgmSource.clip = backGroundMusics[index];
        bgmSource.Play();
    }
    public void UpdateBgmVolume() {
        GameData.bgmVolume = bgmVolumeBar.GetComponent<Slider>().value;
        bgmSource.volume = GameData.bgmVolume;
    }
    public void UpdateSfxVolume() {
        GameData.sfxVolume = sfxVolumeBar.GetComponent<Slider>().value;
        sfxSource.volume = GameData.sfxVolume;
    }
    public void InitialVolume() {
        sfxSource.volume = GameData.sfxVolume;
        bgmSource.volume = GameData.bgmVolume;
        try {
            bgmVolumeBar.GetComponent<Slider>().value = GameData.bgmVolume;
            sfxVolumeBar.GetComponent<Slider>().value = GameData.sfxVolume;
        } catch (System.Exception e) {
            Debug.LogError("Error initializing volume: " + e.Message);
        }
    }
}
