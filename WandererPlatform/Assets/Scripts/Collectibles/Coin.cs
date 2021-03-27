using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isRegistering;
    [SerializeField] private int points;
    [SerializeField] private PoolObjectType poolItemCollectEffectType;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isRegistering) {
            StartCoroutine(nameof(Registering));
            
            PlayItemCollectEffect();

            GameManager.Instance.AddCoins(1);
            GameUI.Instance.ChangeCoinText(GameManager.Instance.GetCoins().ToString());
            SoundManager.Instance.PlaySound(SoundManager.Sound.Coin);
            
            GameManager.Instance.AddScore(points);
            GameUI.Instance.ChangeScoreText(GameManager.Instance.GetScore().ToString());
                
            Destroy(gameObject);
        }
    }

    private IEnumerator Registering() {
        isRegistering = true;
        yield return new WaitForSeconds(.2f);
        isRegistering = false;
    }
    
    private void PlayItemCollectEffect() {
        GameObject itemCollectEffect = PoolManager.Instance.GetPoolObject(poolItemCollectEffectType);
        itemCollectEffect.transform.position = transform.position;
        itemCollectEffect.transform.rotation = transform.rotation;
        itemCollectEffect.SetActive(true);
        itemCollectEffect.GetComponent<ParticleSystem>().Play();
    }
    
    
}
