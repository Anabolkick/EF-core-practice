using System;

namespace People_EF_Core
{
    public class Сourier : Worker
    {
        public Сourier(string name, int salary, int age) : base(name, age, salary)
        {
            Profession = "Сourier";
        }

        public double? DeliverySpeed { get; } = new Random().NextDouble() + 0.5;

        public override void Work()
        {
            Console.WriteLine($"I am working at {Company?.Name}. I will deliver your food!");
        }
    }
}