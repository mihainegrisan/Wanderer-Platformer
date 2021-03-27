using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private bool isRegistering;
    [SerializeField] private int healAmount;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isRegistering) {
            StartCoroutine(nameof(Registering));
            
            other.GetComponent<Player>()?.Heal(healAmount);
            SoundManager.Instance.PlaySound(SoundManager.Sound.Heal);
            Destroy(gameObject);
        }
    }
    
    private IEnumerator Registering() {
        isRegistering = true;
        yield return new WaitForSeconds(.2f);
        isRegistering = false;
    }
}
