using System;
using RandomNameGen;

namespace People_EF_Core
{
    public static class Generators
    {
        private static string GenerateName()
        {
            var rand = new Random();
            var nameGen = new RandomName(rand);
            var name = nameGen.Generate((Sex) rand.Next(2));
            return name;
        }

        public static Worker CreateWorker()
        {
            Console.WriteLine("Pick role:\n 1)Cook\t 2)Courier");
            var profession = (Profession) Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter name of the worker:");
            var name = Console.ReadLine();
            Console.WriteLine("Enter age of the worker:");
            var age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter salary of the worker:");
            var salary = Convert.ToInt32(Console.ReadLine());

            Worker worker;

            switch (profession)
            {
                case Profession.Cook:
                    worker = new Cook(name, salary, age);
                    break;
                case Profession.Courier:
                    worker = new Сourier(name, salary, age);
                    break;
                default:
                    worker = new Cook(name, salary, age); //TODO Exeption
                    Console.WriteLine("ERROR!");
                    break;
            }

            return worker;
        }


        public static Worker GenerateWorker()
        {
            var rand = new Random();
            var profession = (Profession) rand.Next(2);
            Worker worker;

            switch (profession)
            {
                case Profession.Cook:
                    worker = new Cook(GenerateName(), rand.Next(100, 1001), rand.Next(18, 51));
                    break;
                case Profession.Courier:
                    worker = new Сourier(GenerateName(), rand.Next(100, 1001), rand.Next(18, 51));
                    break;
                default:
                    worker = new Cook(GenerateName(), rand.Next(100, 1001), rand.Next(18, 51)); //TODO Exeption
                    Console.WriteLine("ERROR in Name Generator!");
                    break;
            }

            return worker;
        }


        private enum Profession
        {
            Cook,
            Courier
        }
    }
}