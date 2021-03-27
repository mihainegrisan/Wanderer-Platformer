using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime;
    private PlayerMovement playerMovement;

    private void Awake() {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime) {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot() {
        GameObject bullet = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
        bullet.transform.position = firePoint.transform.position;
        bullet.transform.rotation = playerMovement.transform.rotation;
        bullet.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Sound.Shoot);
    }

    
}
