public interface ITurn
{
    void OnStart();
    void OnUpdate();
    void OnEnd();

    void Attack();
    void TakeDamage(int dmg);

    int HP { get; }
    int Speed { get; }
}
