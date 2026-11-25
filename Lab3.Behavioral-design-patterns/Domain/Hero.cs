using Lab3.Behavioral_design_patterns.Patterns;
namespace Lab3.Behavioral_design_patterns.Domain;

public sealed class Hero
{
    // Observer subject stores subscribers through their common interface.
    private readonly List<IHealthObserver> observers = [];
    public string Name { get; }
    public int Health { get; private set; }
    public int MaximumHealth { get; }
    // Replacing this strategy changes attack behavior on the existing hero at runtime.
    public IAttackStrategy Strategy { get; set; }

    internal Hero(string name, int health, IAttackStrategy strategy)
    {
        Name = name;
        Health = MaximumHealth = health;
        Strategy = strategy;
    }

    // Register observers through their interface so any subscriber implementation can join.
    public void Subscribe(IHealthObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!observers.Contains(observer)) observers.Add(observer);
    }
    // Stop future health notifications for this subscriber at runtime.
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
        // Delegate damage calculation to the currently assigned strategy.
        target.ReceiveDamage(Strategy.CalculateDamage());
    }
}
