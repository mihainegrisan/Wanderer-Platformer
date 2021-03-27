using System;

public class Player : LivingEntity {

    protected override void Start() {
        base.Start();

        OnDeath += HealthSystem_OnPlayerDeath;
    }

    private void HealthSystem_OnPlayerDeath(object sender, EventArgs e) {
        GameManager.Instance.ReloadLevel();
    }
}
