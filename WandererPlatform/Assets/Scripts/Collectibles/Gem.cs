using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private bool isRegistering;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isRegistering) {
            StartCoroutine(nameof(Registering));
            SaveData.current.hasGem = true;
            GameUI.Instance.SwitchHasGemImage(true);
            SoundManager.Instance.PlaySound(SoundManager.Sound.Gem);
            Destroy(gameObject);
        }
    }
    
    private IEnumerator Registering() {
        isRegistering = true;
        yield return new WaitForSeconds(.2f);
        isRegistering = false;
    }
}
