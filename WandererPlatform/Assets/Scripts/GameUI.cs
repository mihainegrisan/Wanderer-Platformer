using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    public static GameUI Instance { get; private set; }

    
    [SerializeField] private Slider healthBarUISlider;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image hasGemImage;
    private Player player;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
    }

    private void SceneManager_OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        UpdateUI();
        gameObject.SetActive(SceneManager.GetActiveScene().buildIndex != 0);
        
        player = FindObjectOfType<Player>();
        if (player) {
            player.OnHealthChanged += LivingEntity_OnHealthChanged;
        }
    }

    private void LivingEntity_OnHealthChanged(object sender, EventArgs e) {
        healthBarUISlider.value = player.GetCurrentHealthPercent();
    }
    
    
    public void ChangeCoinText(string text) {
        coinsText.SetText(text);
    }
    
    public void ChangeScoreText(string text) {
        scoreText.SetText($"Score: {text}");
    }

    public void SwitchHasGemImage(bool hasGem) {
        hasGemImage.gameObject.SetActive(hasGem);
    }

    private void UpdateUI() {
        print("UI updated!");
        healthBarUISlider.value = 1;
        ChangeCoinText(GameManager.Instance.GetCoins().ToString());
        ChangeScoreText(GameManager.Instance.GetScore().ToString());
        SwitchHasGemImage(false);
    }
}
