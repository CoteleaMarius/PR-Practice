using System.Net.Http;
using System;
using HttpClientUtmShopClient.Models;
using HttpClientUtmShopClient.Services;
using System.Threading.Tasks;
namespace HttpClientUTMShopClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };

            var categoryService = new CategoryService(client);
            var productService = new ProductService(client);

            while (true)
            {
                Console.WriteLine("\n1. Afiseaza toate categoriile");
                Console.WriteLine("2. Detalii despre categorie");
                Console.WriteLine("3. Creează categorie noua");
                Console.WriteLine("4. Modifica categorie");
                Console.WriteLine("5. Sterge categorie");
                Console.WriteLine("6. Adaugă produs într-o categorie");
                Console.WriteLine("7. Vezi produse într-o categorie");
                Console.WriteLine("0. Iesire");

                Console.Write("\nAlege optiunea: ");
                var opt = Console.ReadLine();

                switch (opt)
                {
                    case "1":
                        var categories = await categoryService.GetCategoriesAsync();
                        categories.ForEach(c => Console.WriteLine($"{c.Id} - {c.Name}"));
                        break;

                    case "2":
                        Console.Write("ID categorie: ");
                        var idDetalii = long.Parse(Console.ReadLine());
                        var detalii = await categoryService.GetCategoryAsync(idDetalii);
                        detalii.ForEach(c => Console.WriteLine($"{c.Id} - {c.Name}"));
                        break;

                    case "3":
                        Console.Write("Titlu nou categorie: ");
                        var titleNou = Console.ReadLine();
                        await categoryService.CreateCategoryAsync(new CreateCategoryDto { Title = titleNou });
                        break;

                    case "4":
                        Console.Write("ID categorie: ");
                        var idEdit = long.Parse(Console.ReadLine());
                        Console.Write("Titlu nou: ");
                        var newName = Console.ReadLine();
                        await categoryService.UpdateCategoryAsync(idEdit, new CreateCategoryDto { Title = newName });
                        break;

                    case "5":
                        Console.Write("ID categorie de sters: ");
                        var idDel = long.Parse(Console.ReadLine());
                        await categoryService.DeleteCategoryAsync(idDel);
                        break;

                    case "6":
                        Console.Write("ID categorie: ");
                        var idCat = long.Parse(Console.ReadLine());
                        Console.Write("Titlu produs: ");
                        var title = Console.ReadLine();
                        Console.Write("Pret produs: ");
                        var price = decimal.Parse(Console.ReadLine());
                        await productService.AddProductToCategoryAsync(idCat, new ProductShortDto { Title = title, Price = price });
                        break;

                    case "7":
                        Console.Write("ID categorie: ");
                        var idProd = long.Parse(Console.ReadLine());
                        var produse = await productService.GetProductsInCategoryAsync(idProd);
                        produse.ForEach(p => Console.WriteLine($"{p.Id}: {p.Title} - {p.Price} RON"));
                        break;

                    case "0":
                        return;
                }
            }
        }
    }
}
