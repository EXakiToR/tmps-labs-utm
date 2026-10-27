namespace Lab2.Three_structural_desgn_patterns;

internal static class DemoChecks
{
    private sealed record TestWeapon(string Name, int Damage) : IWeapon;

    public static int Run()
    {
        int passed = 0;
        void Check(bool condition, string description)
        {
            if (!condition) throw new InvalidOperationException(description);
            Console.WriteLine($"PASS: {description}");
            passed++;
        }
        void Throws<T>(Action action, string description) where T : Exception
        {
            try { action(); }
            catch (T) { Check(true, description); return; }
            throw new InvalidOperationException(description);
        }
        try
        {
            var legacy = new LegacyWand("Oak Wand", 3);
            IWeapon wand = new WandAdapter(legacy);
            Check(wand.Name == legacy.Model && wand.Damage == 18, "Adapter translates the legacy API");
            Throws<ArgumentNullException>(() => new WandAdapter(null!), "Adapter rejects a missing wand");
            Throws<ArgumentException>(() => new LegacyWand(" ", 1), "Legacy wand rejects blank model names");
            Throws<ArgumentOutOfRangeException>(() => new LegacyWand("Wand", 0), "Legacy wand rejects nonpositive power");

            IWeapon enchanted = new PoisonEnchantment(new FireEnchantment(wand));
            Check(enchanted.Name == "Oak Wand + Fire + Poison" && enchanted.Damage == 33, "Decorators stack names and damage");
            Check(wand.Damage == 18 && wand.Name == "Oak Wand", "Decorating preserves the original weapon");
            Check(new FireEnchantment(new Sword()).Damage == 30, "Decorator also wraps a native weapon");
            Check(new FireEnchantment(new PoisonEnchantment(wand)).Damage == 33, "Reversing enchantments preserves additive damage");
            Throws<ArgumentNullException>(() => new FireEnchantment(null!), "Decorator rejects a missing component");

            var rack = new EquipmentRack();
            var arena = new BattleArena();
            var wallet = new GoldWallet();
            var facade = new ExpeditionFacade(rack, arena, wallet);
            QuestResult swordQuest = facade.CompleteQuest(new Sword());
            Check(swordQuest == new QuestResult(new BattleResult(true, 3, 70), 20, 20), "Facade coordinates a sword victory and reward");
            QuestResult magicQuest = facade.CompleteQuest(enchanted);
            Check(magicQuest == new QuestResult(new BattleResult(true, 2, 85), 20, 40), "Enchanted victory accumulates wallet balance");
            Check(ReferenceEquals(rack.EquippedWeapon, enchanted), "Facade equips the supplied weapon");
            QuestResult defeat = facade.CompleteQuest(new TestWeapon("Stick", 1));
            Check(defeat == new QuestResult(new BattleResult(false, 7, 0), 0, 40), "Defeat grants no reward and preserves gold");
            Check(arena.Fight(new TestWeapon("Hammer", 60)) == new BattleResult(true, 1, 100), "Defeated guardians cannot retaliate");
            Throws<ArgumentOutOfRangeException>(() => facade.CompleteQuest(new TestWeapon("Broken", 0)), "Facade rejects zero damage");
            Throws<ArgumentOutOfRangeException>(() => arena.Fight(new TestWeapon("Broken", -1)), "Arena rejects negative damage");
            Throws<ArgumentNullException>(() => facade.CompleteQuest(null!), "Facade rejects missing weapons");
            Check(wallet.Balance == 40, "Invalid quests do not change the wallet");
            Throws<ArgumentOutOfRangeException>(() => wallet.Deposit(-1), "Wallet rejects negative deposits");
            Throws<ArgumentNullException>(() => new ExpeditionFacade(null!, arena, wallet), "Facade requires its subsystems");
            Console.WriteLine($"All {passed} checks passed.");
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine($"FAIL: {error.Message}");
            return 1;
        }
    }
}
