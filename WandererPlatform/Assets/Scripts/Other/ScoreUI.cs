using System;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        ChangeText("Score: 0");
    }

    public void ChangeText(string text) {
        scoreText.SetText($"Score: {text}");
    }
}
