public enum DamageType { None, Melee, Ranged, Burn }

public interface IDamageable {
    
    public bool IsAlive { get; }
    public float Health { get; }
    public float HealthMax { get; }

    public bool Heal(float amount); //Returns true if success

    public bool Damage(float amount, object source, DamageType type = DamageType.None); //Returns true if success

    protected void OnDeath() {}

}
