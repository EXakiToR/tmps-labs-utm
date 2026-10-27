namespace Lab2.Three_structural_desgn_patterns;

// Target interface used by combat and equipment code.
public interface IWeapon
{
    string Name { get; }
    int Damage { get; }
}

public sealed class Sword : IWeapon
{
    public string Name => "Iron Sword";
    public int Damage => 20;
}

// Simulated third-party API: this class does not implement IWeapon.
public sealed class LegacyWand
{
    public string Model { get; }
    private readonly int _powerLevel;

    public LegacyWand(string model, int powerLevel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(powerLevel);
        Model = model.Trim();
        _powerLevel = powerLevel;
    }

    public int CalculateMagicPower() => checked(_powerLevel * 6);
}

// Object adapter: translate the legacy API without modifying its class.
public sealed class WandAdapter : IWeapon
{
    private readonly LegacyWand _wand;

    public WandAdapter(LegacyWand wand)
    {
        ArgumentNullException.ThrowIfNull(wand);
        _wand = wand;
    }

    public string Name => _wand.Model;
    public int Damage => _wand.CalculateMagicPower();
}
