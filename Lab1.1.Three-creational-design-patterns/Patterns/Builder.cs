using System.Collections.Generic;

namespace Lab1._1.Three_creational_design_patterns.Patterns;

public class Product
{
    public string Name { get; init; } = string.Empty;
    public List<string> Parts { get; } = new();

    public override string ToString()
    {
        var parts = Parts.Count == 0 ? "(no parts)" : string.Join(", ", Parts);
        return $"Product(Name={Name}, Parts=[{parts}])";
    }
}

public class ProductBuilder
{
    private Product _product = new();

    public ProductBuilder SetName(string name)
    {
        _product = new Product { Name = name };
        return this;
    }

    public ProductBuilder AddPart(string part)
    {
        _product.Parts.Add(part);
        return this;
    }

    public Product Build()
    {
        // Return an independent copy so the builder can be reused safely
        var copy = new Product { Name = _product.Name };
        copy.Parts.AddRange(_product.Parts);
        return copy;
    }
}
