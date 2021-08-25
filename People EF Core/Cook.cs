using System;

namespace People_EF_Core
{
    public class Cook : Worker
    {
        public Cook(string name, int salary, int age) : base(name, age, salary)
        {
            Profession = "Cook";
        }

        public double? WorkingSpeed { get; } = (new Random().NextDouble() + 1) * 2;

        public override void Work()
        {
            Console.WriteLine($"I am working at {Company?.Name}. I will cook your food."); //TODO No company
        }
    }
}