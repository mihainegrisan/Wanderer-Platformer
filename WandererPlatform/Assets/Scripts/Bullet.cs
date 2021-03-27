using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    
    [SerializeField] private float speed = 20f;
    [SerializeField] private float activeTime;
    [SerializeField] private PoolObjectType poolObjectType;
    [SerializeField] private PoolObjectType poolObjectImpactEffectType;
    
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable() {
        rb.velocity = transform.right * speed;
        StartCoroutine(DeactivateBulletAfter(activeTime));
    }

    private IEnumerator DeactivateBulletAfter(float seconds) {
        yield return new WaitForSeconds(seconds);
        if (enabled) {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("World")) {
            DestroyBullet();
        }
        
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeDamage(damage);
            SoundManager.Instance.PlaySound(SoundManager.Sound.Hit);
            PoolManager.Instance.CoolObject(gameObject, poolObjectType);
        }
        
    }

    private void DestroyBullet() {
        PoolManager.Instance.CoolObject(gameObject, poolObjectType);
        PlayBulletImpactEffect();
    }

    private void PlayBulletImpactEffect() {
        GameObject bulletImpactEffect = PoolManager.Instance.GetPoolObject(poolObjectImpactEffectType);
        bulletImpactEffect.transform.position = transform.position;
        bulletImpactEffect.transform.rotation = transform.rotation;
        bulletImpactEffect.SetActive(true);
    }

    public PoolObjectType GetPoolObjectType() {
        return poolObjectType;
    }

}
