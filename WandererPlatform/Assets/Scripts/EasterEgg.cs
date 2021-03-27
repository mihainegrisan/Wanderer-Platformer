using System;

public class EasterEgg : LivingEntity {
    
    protected override void Start() {
        base.Start();
        
        OnDeath += HealthSystem_OnPlayerDeath; 
    }

    private void HealthSystem_OnPlayerDeath(object sender, EventArgs e) {
        SpawnItems();
        SoundManager.Instance.PlaySound(SoundManager.Sound.EggExplosion);
        Destroy(gameObject);
    }
}
