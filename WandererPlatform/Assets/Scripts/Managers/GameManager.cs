using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManager : MonoBehaviour {
    [SerializeField] private float transitionTime = 2f;
    [SerializeField] private Animator animator;


    public static GameManager Instance { get; private set; }

    private void Awake() {
        //Instance = this;
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        SaveData.current = (SaveData) SaveSystem.Load();
    }

    
#if UNITY_EDITOR
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            LoadNextLevel();
        }
    }
#endif


#region DataMethods

    public void AddScore(int value) {
        SaveData.current.currentScore += value;
    }

    public int GetScore() {
        return SaveData.current.currentScore;
    }

    public void AddCoins(int value) {
        SaveData.current.coins += value;
    }

    public int GetCoins() {
        return SaveData.current.coins;
    }

    public void IncreaseLevel() {
        SaveData.current.level++;
    }

    public int GetCurrentLevel() {
        return SaveData.current.level;
    }

#endregion

    //=========================================================================

#region LevelLoader

    public void ReloadLevel() {
        SaveData.current = (SaveData) SaveSystem.Load();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel() {
        IncreaseLevel();
        SaveSystem.Save(SaveData.current);
        StartCoroutine(LoadLevel(SaveData.current.level));
    }

    private IEnumerator LoadLevel(int levelIndex) {
        animator.Play("Crossfade_Scene_End");
        yield return new WaitForSeconds(transitionTime);
        animator.Play("Crossfade_Scene_Start");
        SceneManager.LoadScene(levelIndex);
    }

    public void ContinueGame() {
        // should be the scene where you left off
        SaveData.current = (SaveData) SaveSystem.Load();
        StartCoroutine(LoadLevel(GetCurrentLevel()));
    }

    public void NewGame() {
        SaveData.current.Reset();
        SaveSystem.Save(SaveData.current);
        ContinueGame();
    }

#endregion
}