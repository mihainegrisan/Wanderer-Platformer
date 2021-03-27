using System;
using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private PoolObjectType poolProjectileType;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime;
    private Enemy enemy;
    private Patrol patrol;
    private Animator animator;

    private static readonly int IsAttacking = Animator.StringToHash("Is_Attacking");
    
    private void Awake() {
        enemy = GetComponentInParent<Enemy>();
        patrol = enemy.GetComponent<Patrol>();
        animator = enemy.GetComponent<Animator>();
    }

    private void Start() {
        patrol.OnAttack += Patrol_OnAttack;
    }

    private void Patrol_OnAttack(object sender, EventArgs e) {
        if (Time.time > nextFireTime) {
            nextFireTime = Time.time + fireRate;
            
            StartCoroutine(nameof(PlayAttackAnimation));
            
            GameObject bullet = PoolManager.Instance.GetPoolObject(poolProjectileType);
            bullet.transform.position = firePoint.transform.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
        }
    }
    
    private IEnumerator PlayAttackAnimation() {
        animator.SetBool(IsAttacking, true);
        yield return new WaitForSeconds(.4f);
        animator.SetBool(IsAttacking, false);
    }


}