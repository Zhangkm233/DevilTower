using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource bgmSource;
    public AudioClip sfx_battle;
    public AudioClip sfx_pick;
    public AudioClip sfx_click;
    public AudioClip sfx_atk;
    public AudioClip sfx_def;
    public AudioClip sfx_door;
    public AudioClip sfx_heal;
    public AudioClip sfx_teleport;
    public AudioClip[] backGroundMusics;
    public AudioClip badEndingMusic;
    public AudioClip forgeMusic;
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
    public void PlayAtk() {
        sfxSource.PlayOneShot(sfx_atk);
    }
    public void PlayDef() {
        sfxSource.PlayOneShot(sfx_def);
    }
    public void PlayDoor() {
        sfxSource.PlayOneShot(sfx_door);
    }
    public void PlayHeal() {
        sfxSource.PlayOneShot(sfx_heal);
    }
    public void PlayTeleport() {
        sfxSource.PlayOneShot(sfx_teleport);
    }
    public void PlayForge() {
        bgmSource.Stop();
        bgmSource.clip = forgeMusic;
        bgmSource.Play();
    }
    public void PlayBadEndBGM() {
        bgmSource.Stop();
        bgmSource.clip = badEndingMusic;
        bgmSource.Play();
    }
    public void PlayBgm(int index) {
        if (index < 0 || index >= backGroundMusics.Length) {
            Debug.LogError("Index out of range");
            return;
        }
        bgmSource.clip = backGroundMusics[index];
        bgmSource.Play();
    }
    public void PlayBgm() {
        bgmSource.Stop();
        PlayBgm(GameData.layer - 1);
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
        SaveManager.LoadForeverData();
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
