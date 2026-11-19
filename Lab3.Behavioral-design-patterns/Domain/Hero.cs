using Lab3.Behavioral_design_patterns.Patterns;
namespace Lab3.Behavioral_design_patterns.Domain;

public sealed class Hero
{
    // Observer subject stores subscribers through their common interface.
    private readonly List<IHealthObserver> observers = [];
    public string Name { get; }
    public int Health { get; private set; }
    public int MaximumHealth { get; }
    public IAttackStrategy Strategy { get; set; }

    internal Hero(string name, int health, IAttackStrategy strategy)
    {
        Name = name;
        Health = MaximumHealth = health;
        Strategy = strategy;
    }

    public void Subscribe(IHealthObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!observers.Contains(observer)) observers.Add(observer);
    }
    public void Unsubscribe(IHealthObserver observer) => observers.Remove(observer);
    public void ReceiveDamage(int damage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damage);
        // Damage and undo share one update path so both notify observers.
        RestoreHealth(Math.Max(0, Health - damage));
    }
    public void RestoreHealth(int health)
    {
        if (health < 0 || health > MaximumHealth) throw new ArgumentOutOfRangeException(nameof(health));
        if (Health == health) return;
        Health = health;
        // A snapshot allows callbacks to subscribe or unsubscribe during notification.
        foreach (var observer in observers.ToArray()) observer.OnHealthChanged(Name, Health);
    }
    public void Attack(Hero target)
    {
        ArgumentNullException.ThrowIfNull(target);
        // Strategy selects the damage algorithm at runtime.
        target.ReceiveDamage(Strategy.CalculateDamage());
    }
}
