using System.Collections;
using UnityEngine;

public class EffectCooldown : MonoBehaviour
{
    [SerializeField] private float activeTime;
    [SerializeField] private PoolObjectType type;
    
    public void OnEnable() {
        StartCoroutine(DeactivateEffectAfter(activeTime));
    }
    
    
    private IEnumerator DeactivateEffectAfter(float seconds) {
        yield return new WaitForSeconds(seconds);
        if (enabled) {
            PoolManager.Instance.CoolObject(gameObject, type);
        }
    }
    
    
}
