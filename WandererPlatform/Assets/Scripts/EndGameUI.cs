using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        DisplayScoreText();
    }

    private void DisplayScoreText() {
        scoreText.SetText(SaveData.current.currentScore.ToString());
    }
}
