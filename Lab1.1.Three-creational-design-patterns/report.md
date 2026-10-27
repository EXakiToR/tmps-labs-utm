# Three creational design patterns implemented

This demo illustrates three creational design patterns: Singleton, Builder, and Factory.

## 1) Singleton
- File: Patterns/Singleton.cs
- Implementation: Thread-safe singleton with double-checked locking.
- Purpose: Provide a globally accessible single instance with simple state (Counter).
- Usage: Singleton.Instance.Increment(); Singleton.Instance.GetInfo();

## 2) Builder
- File: Patterns/Builder.cs
- Implementation: Product and ProductBuilder with fluent API (SetName, AddPart, Build).
- Purpose: Construct complex Product objects step-by-step with readable code.
- Usage: new ProductBuilder().SetName("Gadget").AddPart("Wheel").Build();

## 3) Factory
- File: Patterns/Factory.cs
- Implementation: Simple static ProductFactory with Create(type) returning IFactoryProduct implementations (ConcreteProductA, ConcreteProductB).
- Purpose: Encapsulate object creation and allow clients to obtain different concrete products without coupling to concrete classes.
- Usage: var prod = ProductFactory.Create("A");

## How to run
Build and run the console application. The Program.cs demonstrates each pattern and prints example output to the console.

## Sample output
--- Creational Patterns Demo ---

Singleton demo: Singleton Counter=2
Builder demo: Product(Name=Gadget, Parts=[Wheel, Bolt])
Factory demo - Product A: ConcreteProductA(Name=ProductA)
Factory demo - Product B: ConcreteProductB(Name=ProductB, Version=1)

--- End Demo ---

--- Notes
 - The Builder returns a copy of the internal product to prevent accidental reuse after Build().
 - The Factory encapsulates creation logic; extend ProductFactory when adding new product families.
