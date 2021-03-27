using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }
    
    
    public enum Sound {
        Shoot,
        Hit,
        Heal,
        Coin,
        Gem,
        Jump,
        EnemyExplosion,
        EggExplosion,
        NotPossible,
        Winning,
        MenuClick,
    }

    [SerializeField] private AudioSource musicAudioSource;

    [Header("Menu Theme")]
    [SerializeField] private AudioClip musicMenuTheme;
    
    [Header("Main Theme")]
    [SerializeField] private AudioClip[] musicAudioClips;
    
    [Space]
    [SerializeField] private AudioSource sfxAudioSource;
    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    private int activeClipIndex;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in Enum.GetValues(typeof(Sound))) {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_OnSceneUnloaded;
    }

    private void SceneManager_OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            print("Loaded Menu Scene");
            PlayMenuTheme();
        }
    }
    
    private void SceneManager_OnSceneUnloaded(Scene arg0) {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            print("Unloaded Menu Scene");
            PlayMainTheme();
        }
    }
    
    public void PlaySound(Sound sound) {
        sfxAudioSource.PlayOneShot(soundAudioClipDictionary[sound]);
    }

    private void PlayMainTheme() {
        activeClipIndex %= musicAudioClips.Length;
        //int previousClipIndex = activeClipIndex == 0 ? musicAudioClips.Length : activeClipIndex - 1 ;
        if (musicAudioSource) {
            musicAudioSource.clip = musicAudioClips[activeClipIndex];
            musicAudioSource.Play();
        }
        
        activeClipIndex++;
    }

    private void PlayMenuTheme() {
        if (musicAudioSource) {
            musicAudioSource.clip = musicMenuTheme;
            musicAudioSource.Play();
        }
    }
    
}
