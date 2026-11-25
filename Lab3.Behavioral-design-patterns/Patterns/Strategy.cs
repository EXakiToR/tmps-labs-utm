namespace Lab3.Behavioral_design_patterns.Patterns;

public interface IAttackStrategy
{
    // All attack algorithms supply damage through this interchangeable operation.
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
