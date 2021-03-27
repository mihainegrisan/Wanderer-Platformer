using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    private Slider healthBarUISlider;
    private Player player;

    private void Start() {
        healthBarUISlider = GetComponent<Slider>();
        player = FindObjectOfType<Player>();
        player.OnHealthChanged += LivingEntity_OnHealthChanged;
    }

    private void LivingEntity_OnHealthChanged(object sender, EventArgs e) {
        healthBarUISlider.value = player.GetCurrentHealthPercent();
    }
    
    
}
