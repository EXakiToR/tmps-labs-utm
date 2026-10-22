namespace Lab1._1.Three_creational_design_patterns.Patterns;

public interface IFactoryProduct
{
    string Describe();
}

public class ConcreteProductA : IFactoryProduct
{
    public string Name { get; set; } = "ProductA";
    public override string ToString() => Describe();
    public string Describe() => $"ConcreteProductA(Name={Name})";
}

public class ConcreteProductB : IFactoryProduct
{
    public string Name { get; set; } = "ProductB";
    public int Version { get; set; } = 1;
    public override string ToString() => Describe();
    public string Describe() => $"ConcreteProductB(Name={Name}, Version={Version})";
}

public static class ProductFactory
{
    // Simple factory method
    public static IFactoryProduct Create(string type)
    {
        return type switch
        {
            "A" => new ConcreteProductA(),
            "B" => new ConcreteProductB(),
            _ => throw new ArgumentException($"Unknown product type: {type}", nameof(type)),
        };
    }
}
