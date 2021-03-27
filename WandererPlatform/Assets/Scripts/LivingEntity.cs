using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class LivingEntity : MonoBehaviour, IDamageable {
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDeath;
    
    [SerializeField] private GameObject[] spawnItemPrefabArray;
    [SerializeField] private GameObject container;
    [SerializeField] private int healthMax;
    private int currentHealth;
    private const float InvincibleTime = .2f;
    private bool isInvincible;
    private Vector2 pos;
    [SerializeField] private Vector2 itemSpawnRadius;
    [SerializeField] private Vector2 itemsToSpawnMinMax;
    
    [SerializeField] private LayerMask layerMask;

    protected virtual void Start() {
        currentHealth = healthMax;
        pos = transform.position;
    }

    public void TakeDamage(int damageAmount) {
        if (isInvincible) {
            return;
        }
        
        currentHealth -= damageAmount;
        //print(GetCurrentHealth());
        StartCoroutine(nameof(BecomeInvincible));
        
        if (currentHealth <= 0) {
            currentHealth = 0;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
        
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    // private int GetCurrentHealth() {
    //     return currentHealth;
    // }

    public float GetCurrentHealthPercent() {
        return (float)currentHealth / healthMax;
    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        
        if (currentHealth > healthMax) {
            currentHealth = healthMax;
        }
        
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private IEnumerator BecomeInvincible() {
        isInvincible = true;
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible = false;
    }
    
    protected void SpawnItems() {
        // It can spawn 0 items no matter the minimum!
        int numberOfItemsToSpawn = (int) Random.Range(itemsToSpawnMinMax.x,itemsToSpawnMinMax.y);
        //print("1. totalNumberOfItems: " + numberOfItemsToSpawn);

        foreach (GameObject pfSpawnItem in spawnItemPrefabArray) {
            int itemsOfThisTypeToSpawn = Random.Range(0, numberOfItemsToSpawn);
            //print($"Item: {pfSpawnItem} ... Spawn: {itemsOfThisTypeToSpawn}");
            
            
            CircleCollider2D circleCollider2D = pfSpawnItem.GetComponent<CircleCollider2D>();
            float radius = circleCollider2D.radius;
            //print($"2. Radius: {radius}");

            while (itemsOfThisTypeToSpawn > 0) {
                int numberOfCollidersHit;
                Collider2D[] collidersHit = new Collider2D[2];
                Vector2 futureSpawnPosition;

                do {
                    futureSpawnPosition = new Vector2(
                        Random.Range(pos.x - itemSpawnRadius.x, pos.x + itemSpawnRadius.x), 
                        Random.Range(pos.y - .5f, pos.y + itemSpawnRadius.y));
                      
                    //print($"Loop.3. Random spawn position: {futureSpawnPosition}");
                    //collidersHit = Physics2D.OverlapCircleAll(futureSpawnPosition, radius, layerMask);
                    numberOfCollidersHit = Physics2D.OverlapCircleNonAlloc(futureSpawnPosition, radius, collidersHit, layerMask);
                    //print($"Loop.4. Number of colliders hit: {numberOfCollidersHit}");
                } while (numberOfCollidersHit > 0);
                
                //print($"5. Final Spawn Position: {futureSpawnPosition}");
                Instantiate(pfSpawnItem, futureSpawnPosition, Quaternion.identity, container.transform);
                itemsOfThisTypeToSpawn--;
            }
            
            numberOfItemsToSpawn -= itemsOfThisTypeToSpawn;
        }
    }
}
