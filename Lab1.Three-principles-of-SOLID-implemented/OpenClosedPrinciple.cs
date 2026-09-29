using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.Three_principles_of_SOLID_implemented;

// Demonstrates the Open/Closed Principle (OCP)
// AreaCalculator is closed for modification but open for extension via the IShape abstraction.
public interface IShape
{
    double Area();
}

public class Rectangle : IShape
{
    private double Width { get; }
    private double Height { get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double Area() => Width * Height;
}

public class Circle : IShape
{
    private double Radius { get; }

    public Circle(double radius) => Radius = radius;

    public double Area() => Math.PI * Radius * Radius;
}

public class AreaCalculator
{
    public double TotalArea(IEnumerable<IShape> shapes)
    {
        if (shapes == null) throw new ArgumentNullException(nameof(shapes));
        return shapes.Sum(s => s.Area());
    }
}
