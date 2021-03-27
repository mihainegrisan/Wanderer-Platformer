using System;
using System.Collections;
using UnityEngine;

public class Enemy : LivingEntity {
    //private Animator animator;
    //private static readonly int WasHit = Animator.StringToHash("Was_Hit");
    [SerializeField] private float timeBetweenContinuousDamageReceived = 1f;
    private float nextDamageTime;
    
    protected override void Start() {
        base.Start();

        //animator = GetComponent<Animator>();
        
        OnDeath += HealthSystem_OnDeath; 
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            IDamageable damageableObject = other.gameObject.GetComponent<IDamageable>();
            if (damageableObject != null && Time.time > nextDamageTime) {
                damageableObject.TakeDamage(20);
                SoundManager.Instance.PlaySound(SoundManager.Sound.Hit);
                nextDamageTime = Time.time + timeBetweenContinuousDamageReceived;
            }
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        PlayEnemyDeathEffect();
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyExplosion);
        SpawnItems();
        Destroy(gameObject);
    }

    private void PlayEnemyDeathEffect() {
        GameObject enemyDeathEffect = PoolManager.Instance.GetPoolObject(PoolObjectType.EnemyDeathEffect);
        enemyDeathEffect.transform.position = transform.position;
        enemyDeathEffect.transform.rotation = transform.rotation;
        enemyDeathEffect.SetActive(true);
    }
    
    // private IEnumerator PlayWasHitAnimation() {
    //     animator.SetBool(WasHit, true);
    //     yield return new WaitForSeconds(.3f);
    //     animator.SetBool(WasHit, false);
    // }

}
