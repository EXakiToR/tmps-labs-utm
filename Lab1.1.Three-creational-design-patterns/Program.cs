using Lab1._1.Three_creational_design_patterns.Patterns;

namespace Lab1._1.Three_creational_design_patterns;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- Creational Patterns Demo ---\n");

        // Singleton demo
        var s1 = Singleton.Instance;
        s1.Increment();
        var s2 = Singleton.Instance;
        s2.Increment();
        Console.WriteLine("Singleton demo: " + s1.GetInfo());

        // Builder demo
        var builder = new ProductBuilder();
        var product = builder.SetName("Gadget").AddPart("Wheel").AddPart("Bolt").Build();
        Console.WriteLine("Builder demo: " + product);

        // Factory demo
        var prodA = ProductFactory.Create("A");
        var prodB = ProductFactory.Create("B");
        Console.WriteLine("Factory demo - Product A: " + prodA.Describe());
        Console.WriteLine("Factory demo - Product B: " + prodB.Describe());

        Console.WriteLine("\n--- End Demo ---");
    }
}
