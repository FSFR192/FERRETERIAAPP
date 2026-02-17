using System.Text.Json;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
namespace FerreteriaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FerreteriaApi");
            var response = await client.GetAsync("api/productos");

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductoViewModel>());

            var json = await response.Content.ReadAsStringAsync();

            var productos = JsonSerializer.Deserialize<List<ProductoViewModel>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return View(productos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoViewModel producto)
        {
            var client = _httpClientFactory.CreateClient("FerreteriaApi");

            var response = await client.PostAsJsonAsync("api/productos", producto);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("STATUS: " + response.StatusCode);
            Console.WriteLine("ERROR: " + result);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            Console.WriteLine(producto.Nombre);
            Console.WriteLine(producto.Precio);


            return View(producto);
        }

    }
}