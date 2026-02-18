using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using FerreteriaWeb.Models;

public class VentasController : Controller
{
    private readonly HttpClient _httpClient;

    public VentasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5248/api/");
    }

    public async Task<IActionResult> Create()
    {
        var response = await _httpClient.GetAsync("Productos");

        var productosJson = await response.Content.ReadAsStringAsync();
        var productos = JsonSerializer.Deserialize<List<Producto>>(productosJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        ViewBag.Productos = new SelectList(productos, "Id", "Nombre");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(VentaViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var venta = new
        {
            detalles = new[]
            {
                new
                {
                    productoId = model.ProductoId,
                    cantidad = model.Cantidad
                }
            }
        };

        var json = JsonSerializer.Serialize(venta);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("Ventas", content);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Venta registrada correctamente";
            return RedirectToAction("Index", "Productos");
        }

        TempData["Error"] = "Error al registrar venta";
        return View(model);
    }
}
