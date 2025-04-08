using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource bgmSource;
    public AudioClip sfx_battle;
    public AudioClip sfx_pick;
    public AudioClip sfx_click;
    public AudioClip[] backGroundMusics;
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
    public void UpdateVolume() {
        sfxSource.volume = GameData.sfxVolume;
        bgmSource.volume = GameData.bgmVolume;
    }
    public void ChangeSfxVolume(float volume) {
        sfxSource.volume = volume;
        GameData.sfxVolume = volume;
    }
}
