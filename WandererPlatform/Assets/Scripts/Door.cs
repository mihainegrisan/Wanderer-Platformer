using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isRegistering;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isRegistering) {
            StartCoroutine(nameof(Registering));
            
            SoundManager.Instance.PlaySound(SoundManager.Sound.Winning);
            
            if (SaveData.current.hasGem) {
                SaveData.current.hasGem = false;
                GameUI.Instance.SwitchHasGemImage(false);
                GameManager.Instance.LoadNextLevel();
            }
            else {
                SoundManager.Instance.PlaySound(SoundManager.Sound.NotPossible);
            }
        }
    }
    
    private IEnumerator Registering() {
        isRegistering = true;
        yield return new WaitForSeconds(.2f);
        isRegistering = false;
    }
}
