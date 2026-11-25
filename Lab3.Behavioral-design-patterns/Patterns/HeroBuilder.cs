using Lab3.Behavioral_design_patterns.Domain;
namespace Lab3.Behavioral_design_patterns.Patterns;

public sealed class HeroBuilder
{
    private string name = "Hero";
    private int health = 100;
    private IAttackStrategy strategy = new SwordAttack();
    // Configure the name and return this builder so construction steps can be chained.
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
    // Choose the initial attack algorithm for the hero being constructed.
    public HeroBuilder WithStrategy(IAttackStrategy value)
    {
        ArgumentNullException.ThrowIfNull(value);
        strategy = value;
        return this;
    }
    // Complete construction by creating a new hero from the configured name, health, and strategy.
    public Hero Build() => new(name, health, strategy);
}
