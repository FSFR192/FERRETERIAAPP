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

        //EDITAR PRODUCTO
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("FerreteriaApi");

            var response = await client.GetAsync($"api/productos/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();

            var producto = JsonSerializer.Deserialize<ProductoViewModel>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductoViewModel producto)
        {
            var client = _httpClientFactory.CreateClient("FerreteriaApi");
            var response = await client.PutAsJsonAsync(
                $"api/productos/{producto.Id}", producto);

            if (response.IsSuccessStatusCode)
            TempData["Success"] = "Producto actualizado correctamente.";
                return RedirectToAction("Index");

            return View(producto);
        }

        

        //CREAR PRODUCTO
        
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
                TempData["Success"] = "Producto creado correctamente.";
                return RedirectToAction("Index");

            Console.WriteLine(producto.Nombre);
            Console.WriteLine(producto.Precio);


            return View(producto);
        }

        //ELIMINAR PRODUCTO
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("FerreteriaApi");

            var response = await client.DeleteAsync($"api/productos/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Producto eliminado correctamente.";
            return RedirectToAction("Index");
        }

    }
}