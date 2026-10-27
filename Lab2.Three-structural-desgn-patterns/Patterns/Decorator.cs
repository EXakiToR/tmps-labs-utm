namespace Lab2.Three_structural_desgn_patterns;

public abstract class WeaponDecorator : IWeapon
{
    protected IWeapon Inner { get; }

    protected WeaponDecorator(IWeapon inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        Inner = inner;
    }

    public abstract string Name { get; }
    public abstract int Damage { get; }
}

public sealed class FireEnchantment : WeaponDecorator
{
    public FireEnchantment(IWeapon inner) : base(inner) { }
    public override string Name => $"{Inner.Name} + Fire";
    public override int Damage => checked(Inner.Damage + 10);
}

public sealed class PoisonEnchantment : WeaponDecorator
{
    public PoisonEnchantment(IWeapon inner) : base(inner) { }
    public override string Name => $"{Inner.Name} + Poison";
    public override int Damage => checked(Inner.Damage + 5);
}
