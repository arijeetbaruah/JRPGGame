public interface ITurn
{
    void OnStart();
    void OnUpdate();
    void OnEnd();

    void Attack();
    void TakeDamage(int dmg);

    void AddStatusEffect(StatusEffect statusEffect);
    void ApplyStatusToTarget(StatusEffect statusEffect);

    int HP { get; }
    int Speed { get; }
}
