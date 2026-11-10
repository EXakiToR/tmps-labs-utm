namespace Lab2.Three_structural_desgn_patterns;

public interface IWeaponFactory
{
    IWeapon CreateSword();
    IWeapon CreateWand(string model, int powerLevel);
}

public sealed class WeaponFactory : IWeaponFactory
{
    public IWeapon CreateSword() => new Sword();

    public IWeapon CreateWand(string model, int powerLevel) =>
        new WandAdapter(new LegacyWand(model, powerLevel));
}
