using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BankClientApplication
{
    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
    class Program
    {
        static void Main()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient()) // met het gebruik van using zorgen we ervoor dat alles netjes wordt opgeruimd als je buiten de scope zit. 
            {
                client.BaseAddress = new Uri("http://localhost:50752/"); //de URL waar onze website op draait
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //we krijgen JSON terug van onze API

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/RekeningsApi/1"); //ik wil de eerste klant ophalen. Deze bevindt zich in de api, de RekeningsApiController en dan nummer 1
                if (response.IsSuccessStatusCode) //Controleren of we geen 404 krijgen, maar iets in de 200-reeks
                {
                    Rekening rekening = await response.Content.ReadAsAsync<Rekening>(); //zetten de response asynchroon om in een rekening-object
                    Console.WriteLine("Rekeningnaam: {0}\tBeschrijving: {1}\tSaldo: {2}", rekening.RekeningNaam, rekening.Beschrijving, rekening.Saldo); //en schrijven deze naar de console
                }
                Console.ReadKey(); //even op een toets drukken om verder te gaan


                var klantNew = new Klant() { Beschrijving = "willekeurige klant", Name = "John Doe" }; //we maken een nieuwe klant aan en vullen deze meteen met info
                response = await client.PostAsJsonAsync("api/KlantsApi/", klantNew); //we sturen deze nieuwe klant asynchroon naar de api via een HTTP Post

                if (response.IsSuccessStatusCode) //check of we geen 404 of iets dergelijks krijgen
                {
                    Console.ReadKey();
                    
                }

                // HTTP POST
                //let op: je kan pas een rekening aanmaken als je de klant eerst hebt aangemaakt vanwege je constraints; een rekening die niet bij een klant hoort zou natuurlijk raar zijn.
                var RekeningNew = new Rekening() { Beschrijving = "Nieuw product", RekeningNaam = "zilvervloot-rekening", Saldo = 5000.50, KlantId=2};//nieuwe rekening aanmaken

         
                
                response = await client.PostAsJsonAsync("api/RekeningsApi/", RekeningNew);
                if (response.IsSuccessStatusCode) //check of we geen 404 of iets dergelijks krijgen
                {
                  Console.WriteLine("klant aangemaakt");
                  Console.ReadKey();
               //     Uri responseUrl = response.Headers.Location;

                    // HTTP PUT
               //     RekeningNew.Saldo = 80;   // Update price
               //     response = await client.PutAsJsonAsync(responseUrl, RekeningNew);

                    // HTTP DELETE
                    //response = await client.DeleteAsync(responseUrl);
                }
            }
        }
    }
}
