using Lab1._1.Three_creational_design_patterns.Patterns;

namespace Lab1._1.Three_creational_design_patterns;

internal static class DemoChecks
{
    public static int Run()
    {
        int passed = 0;
        void Check(bool condition, string description)
        {
            if (!condition) throw new InvalidOperationException(description);
            passed++;
            Console.WriteLine($"PASS: {description}");
        }
        void Throws<T>(Action action, string description) where T : Exception
        {
            try { action(); }
            catch (T) { Check(true, description); return; }
            throw new InvalidOperationException(description);
        }
        try
        {
            var instances = new GameJournal[32];
            Parallel.For(0, instances.Length, i => instances[i] = GameJournal.Instance);
            Check(instances.All(j => ReferenceEquals(j, instances[0])), "Concurrent callers share one journal");
            var journal = GameJournal.Instance;
            int before = journal.GetEntries().Count;
            var snapshot = journal.GetEntries();
            Parallel.For(0, 32, i => journal.Record($"Event {i}"));
            Check(journal.GetEntries().Count == before + 32, "Concurrent writes retain every event");
            Check(snapshot.Count == before, "Journal snapshots remain independent");
            Throws<ArgumentException>(() => journal.Record(" "), "Journal rejects empty entries");
            IHeroBuilder builder = new HeroBuilder().Named("Test").AddEquipment("Sword");
            Hero first = builder.Build();
            Hero second = builder.Named("Other").WithHealth(80).AddEquipment("Shield").Build();
            Check(first.Name == "Test" && first.Health == 100 && first.Equipment.Count == 1
                && second.Name == "Other" && second.Health == 80 && second.Equipment.Count == 2,
                "Builder reuse preserves earlier heroes");
            Throws<NotSupportedException>(() => ((IList<string>)first.Equipment).Add("Axe"), "Hero equipment is read-only");
            Throws<InvalidOperationException>(() => new HeroBuilder().Build(), "Builder requires a name");
            Throws<ArgumentException>(() => builder.Named(" "), "Builder rejects blank names");
            Throws<ArgumentException>(() => builder.AddEquipment(" "), "Builder rejects blank equipment");
            Throws<ArgumentOutOfRangeException>(() => builder.WithHealth(0), "Builder rejects zero health");
            Throws<ArgumentOutOfRangeException>(() => builder.WithAttack(-1), "Builder rejects negative attack");
            Encounter forest = new ForestEncounter();
            Encounter mountain = new MountainEncounter();
            Check(forest.CreateEnemy() is Goblin && mountain.CreateEnemy() is Dragon, "Factories choose correct products");
            Check(!ReferenceEquals(forest.CreateEnemy(), forest.CreateEnemy()), "Factories create fresh enemies");
            Hero knight = HeroDirector.CreateKnight(new HeroBuilder());
            Check(forest.Play(knight) == new BattleResult(true, 92, 10), "Forest victory health and reward");
            Check(mountain.Play(knight) == new BattleResult(true, 40, 50), "Mountain victory health and reward");
            Hero weak = new HeroBuilder().Named("Novice").WithHealth(1).WithAttack(1).Build();
            Check(mountain.Play(weak) == new BattleResult(false, 0, 0), "Defeat clamps health and grants no reward");
            Hero strong = new HeroBuilder().Named("Champion").WithHealth(1).WithAttack(100).Build();
            Check(mountain.Play(strong) == new BattleResult(true, 1, 50), "Defeated enemies cannot retaliate");
            Throws<ArgumentNullException>(() => forest.Play(null!), "Encounters reject a missing hero");
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
