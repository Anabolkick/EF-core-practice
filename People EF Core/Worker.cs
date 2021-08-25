namespace People_EF_Core
{
    public abstract class Worker
    {
        protected Worker(string name, int age, int salary)
        {
            Name = name;
            Age = age;
            Salary = salary;
        }

        public int Id { get; private set; }
        public string Name { get; }
        public int Age { get; }
        public string Profession { get; set; } = "Without profession"; //TODO ????
        public int Salary { get; }

        public int? CompanyId { get; private set; } // внешний ключ
        public Company Company { get; private set; } // навигационное свойство

        public abstract void Work();
    }
}