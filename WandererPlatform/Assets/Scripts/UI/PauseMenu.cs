using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool gameIsPaused;
    [SerializeField] private GameObject pauseMenu;

    private void Start() {
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
    }

    private void SceneManager_OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        Resume();
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        Application.Quit();
    }
}
