namespace Lab3.Behavioral_design_patterns.Patterns;

// Hero delegates damage calculation to interchangeable implementations.
public interface IAttackStrategy
{
    int CalculateDamage();
}
public sealed class SwordAttack : IAttackStrategy
{
    public int CalculateDamage() => 20;
}
public sealed class MagicAttack : IAttackStrategy
{
    public int CalculateDamage() => 35;
}
