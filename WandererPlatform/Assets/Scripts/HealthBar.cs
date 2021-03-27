using System;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    private Transform healthBar;
    private Quaternion initialRotation;
    private LivingEntity livingEntity;

    private void Awake() {
        initialRotation = transform.rotation;
    }

    private void Start() {
        healthBar = transform.Find("Bar");
        healthBar.parent.gameObject.SetActive(false);
        livingEntity = GetComponentInParent<LivingEntity>();
        livingEntity.OnHealthChanged += LivingEntity_OnHealthChanged;
    }

    private void LateUpdate() {
        transform.rotation = initialRotation;
    }

    private void LivingEntity_OnHealthChanged(object sender, EventArgs e) {
        healthBar.parent.gameObject.SetActive(true);
        healthBar.localScale = new Vector3(livingEntity.GetCurrentHealthPercent(), 1);
    }

    
}