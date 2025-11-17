public enum DamageType { None, Melee, Ranged, Burn }

public interface IDamageable {
    
    public bool IsAlive { get; }
    public float Health { get; }
    public float HealthMax { get; }

    protected void OnDeath() {}

    public bool Revive(float health); //Returns true if success

    public bool Heal(float amount); //Returns true if success

    public bool Damage(float amount, DamageType type, object source); //Returns true if success

}
