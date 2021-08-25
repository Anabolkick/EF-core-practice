using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace People_EF_Core
{
    internal class Program
    {
        private static void Main(string[] args) //TODO if User Login allready exist
        {
            var pass1 = HashPassword("1111");
            var user1 = new User("Anab", pass1.password, pass1.salt);

            var pass2 = HashPassword("56545");
            var user2 = new User("Lone", pass2.password, pass2.salt);

            var workers = new List<Worker>();

            var amazon = user1.CreateCompany("Amazon", 500000);
            var google = user1.CreateCompany("Google", 500000);
            var facebook = user1.CreateCompany("Facebook", 500000);
            var hyperx = user1.CreateCompany("Hyperx", 500000);
            var rockstar = user1.CreateCompany("Rockstar", 500000);

            var companies = new List<Company> {amazon, google, facebook, hyperx, rockstar};

            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 700; i++)
            {
                workers.Add(Generators.GenerateWorker());
                Thread.Sleep(100);
                companies[rand.Next(companies.Count)].AddWorker(workers[i]);
            }

            using (var db = new AppContext())
            {
                db.Companies.AddRange(companies);
                // db.Workers.AddRange(workers);
                db.User.AddRange(user1, user2);

                db.SaveChanges();
            }

            Login_mark:
            var user = Login();

            if (user == null)
            {
                Console.WriteLine("Please, try again.");
                goto Login_mark;
            }

            Console.WriteLine($"{user.Login} Welcome!!!");

            #region Test
            //using (var db = new AppContext())
            //{
            //    #region Avg Group Salory

            //    var _workers = db.Workers.Include(w => w.Company).ToList();
            //    var groups = _workers.GroupBy(w => w.Company.Name).ToList()
            //        .OrderByDescending(w => w.Average(w => w.Salary)).ToList();
            //    // groups = groups.OrderByDescending(w => w.Average(w => w.Salary)).ToList();
            //    foreach (var group in groups)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Cyan;
            //        Console.WriteLine(group.Key);
            //        Console.ResetColor();
            //        var avg = group.Average(w => w.Salary);

            //        Console.WriteLine(avg);
            //        Console.WriteLine();
            //    }

            //    #endregion

            //    var _workers = (from worker in db.Workers.Include(w => w.Company)
            //        where worker.Company.Name == "Amazon" && worker.Salary < 400
            //        select worker).OrderBy(w => w.Salary).ToList();


            //    var _workers = db.Workers.Include(w => w.Company)
            //        .Where(w => w.Company.Name == "Amazon" && w.Salary < 400)
            //        .OrderBy(w => w.Salary).ToList();

            //    var _workers = db.Workers.Include(w => w.Company)
            //        .Where(w => EF.Functions.Like(w.Name, "[^ui]"))
            //        .OrderBy(w => w.Name).ToList();

            //    var _workers = db.Workers.Select(w => new
            //    {
            //        w.Id,
            //        w.Name,
            //        Company = w.Company.Name
            //    });

            //    var number = 0;
            //    foreach (var w in _workers)
            //    {
            //        number++;
            //        Console.WriteLine($"{number}) {w.Name} ({w.Id}) - Company: {w.Company}.");
            //        // w.Work();
            //        Console.WriteLine();
            //    }


            //    var number = 0;
            //    foreach (Worker w in _workers)
            //    {
            //        number++;
            //        Console.WriteLine($"{number}) {w.Name} ({w.Id}) - Company: {w.Company.Name}, salary: {w.Salary}");
            //        // w.Work();
            //        Console.WriteLine();
            //    }
            //}
            #endregion
        }

        private static User Login()
        {
            Console.WriteLine("Enter your login:");
            var login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            var password = Console.ReadLine();

            User user;

            using (var db = new AppContext())
            {
                var salt = db.User.Where(u => u.Login == login).Select(u => u.Salt).FirstOrDefault();

                if (salt != null)
                    password = HashPassword(password, salt).password;
                else
                    return null; //TODO something

                user = db.User.FirstOrDefault(u => u.Login == login && u.Password == password);
            }

            return user;
        }

        private static (string password, byte[] salt) HashPassword(string password)
        {
            var salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2
            (
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                256 / 8
            ));

            return (hashed, salt);
        }

        private static (string password, byte[] salt) HashPassword(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2
            (
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                256 / 8
            ));

            return (hashed, salt);
        }
    }
}