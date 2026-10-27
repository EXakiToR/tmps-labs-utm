namespace Lab2.Three_structural_desgn_patterns;

internal static class Program
{
    private static int Main(string[] args)
    {
        if (args is ["--self-test"]) return DemoChecks.Run();
        if (args.Length != 0)
        {
            Console.Error.WriteLine("Usage: dotnet run -- [--self-test]");
            return 1;
        }

        Console.WriteLine("=== Enchanted Expedition: Structural Patterns ===");
        IWeapon sword = new Sword();
        IWeapon wand = new WandAdapter(new LegacyWand("Oak Wand", 3));
        Console.WriteLine("\n[Adapter] Use a legacy wand through IWeapon:");
        Console.WriteLine($"{wand.Name}: {wand.Damage} damage");

        IWeapon enchanted = new PoisonEnchantment(new FireEnchantment(wand));
        Console.WriteLine("\n[Decorator] Stack enchantments without changing the wand:");
        Console.WriteLine($"{enchanted.Name}: {enchanted.Damage} damage");
        Console.WriteLine($"Original {wand.Name}: {wand.Damage} damage");

        var expedition = new ExpeditionFacade(new EquipmentRack(), new BattleArena(), new GoldWallet());
        Console.WriteLine("\n[Facade] Equip, fight a guardian, and collect the reward:");
        foreach (IWeapon weapon in new[] { sword, enchanted })
        {
            QuestResult result = expedition.CompleteQuest(weapon);
            Console.WriteLine($"{weapon.Name}: Victory={result.Battle.Won}, Turns={result.Battle.Turns}, HP={result.Battle.RemainingHealth}, Gold earned={result.GoldEarned}, Wallet={result.TotalGold}");
        }
        return 0;
    }
}
