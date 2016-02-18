using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer2
{
    class Program
    {
        static void Main(string[] args)
        {
            //we maken een nieuwe database-context. Als de database nog niet bestaat; wordt deze aangemaakt
            using (var db = new BankContext())
            {

                Console.WriteLine("Voer de naam van de eerste klant in:");
                string name = Console.ReadLine();

                //we maken een nieuw klant-object aan
                var Client = new Client { Name = name };

                //we we voegen deze klant aan de database toe
                db.Clients.Add(Client);

                //en we slaan de nieuwe dingen in de database op
                db.SaveChanges();

                Console.WriteLine("\nDe klanten zijn:");
                foreach (var item in db.Clients)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine();
                Console.WriteLine("please enter the name of your new bank account");
                var account = Console.ReadLine();
                var Account = new Account { Client = Client, Title = account};
                db.Accounts.Add(Account);
                db.SaveChanges();
                Console.WriteLine("\nDe rekeningen zijn:");
                foreach (var item in db.Accounts)
                {
                    Console.WriteLine("rekeningnaam: " + item.Title);
                    Console.WriteLine("eigenaar: " + item.Client.Name);
                    Console.WriteLine();
                }

                Console.WriteLine("Voer de naam in van de klant die je wilt zoeken");
                var zoekstring = Console.ReadLine();
                var GevondenKlant = db.Clients.Where(x => x.Name.Contains(zoekstring)).First();
                Console.Write(GevondenKlant.Name);
                Console.ReadKey();
            }
                
        }
    }
    public class BankContext : DbContext
    {
        public BankContext() : base("MyDbContextConnectionString") { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Beschrijving { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }

    public class Account
    {
        public int AccountId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
    
}
