using Lab3.Behavioral_design_patterns.Domain;
namespace Lab3.Behavioral_design_patterns.Patterns;

public sealed class HeroBuilder
{
    private string name = "Hero";
    private int health = 100;
    private IAttackStrategy strategy = new SwordAttack();
    public HeroBuilder Named(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        name = value.Trim();
        return this;
    }
    public HeroBuilder WithHealth(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        health = value;
        return this;
    }
    public HeroBuilder WithStrategy(IAttackStrategy value)
    {
        ArgumentNullException.ThrowIfNull(value);
        strategy = value;
        return this;
    }
    // Each build creates a separate hero using the configured values.
    public Hero Build() => new(name, health, strategy);
}
