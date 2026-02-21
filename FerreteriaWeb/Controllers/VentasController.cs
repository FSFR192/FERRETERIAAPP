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
public async Task<IActionResult> Create(CrearVentaViewModel model)
{
    if (!ModelState.IsValid)
    {
        // Recargar productos para el dropdown
        var productosResp = await _httpClient.GetAsync("Productos");
        var productosJson = await productosResp.Content.ReadAsStringAsync();
        var productos = JsonSerializer.Deserialize<List<Producto>>(productosJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Productos = new SelectList(productos, "Id", "Nombre");

        TempData["Error"] = "Datos inválidos: " + string.Join(", ", 
            ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return View(model);
    }

    var venta = new
    {
        detalles = new[] { new { productoId = model.ProductoId, cantidad = model.Cantidad } }
    };

    var json = JsonSerializer.Serialize(venta);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await _httpClient.PostAsync("Ventas", content);

    if (response.IsSuccessStatusCode)
    {
        var productoResp = await _httpClient.GetAsync($"Productos/{model.ProductoId}");
        var productoJson = await productoResp.Content.ReadAsStringAsync();
        var producto = JsonSerializer.Deserialize<ProductoViewModel>(productoJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        TempData["Boleta_Producto"] = producto.Nombre;
        TempData["Boleta_Cantidad"] = model.Cantidad;
        TempData["Boleta_PrecioUnitario"] = producto.Precio.ToString("0.00");
        TempData["Boleta_Total"] = (model.Cantidad * producto.Precio).ToString("0.00");
        TempData["Boleta_Fecha"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        return RedirectToAction("Boleta");
    }

    // Ver qué devuelve la API
    var errorMsg = await response.Content.ReadAsStringAsync();
    TempData["Error"] = $"Error API: {errorMsg}";

    var productosResp2 = await _httpClient.GetAsync("Productos");
    var productosJson2 = await productosResp2.Content.ReadAsStringAsync();
    var productos2 = JsonSerializer.Deserialize<List<Producto>>(productosJson2,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    ViewBag.Productos = new SelectList(productos2, "Id", "Nombre");

    return View(model);
}

    public IActionResult Boleta()
    {
        return View();
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("Ventas");  // ← CORREGIDO
        if (!response.IsSuccessStatusCode)
            return View(new List<VentaViewModel>());

        var ventasJson = await response.Content.ReadAsStringAsync();  // ← CORREGIDO
        var ventas = JsonSerializer.Deserialize<List<VentaViewModel>>(ventasJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(ventas ?? new List<VentaViewModel>());
    }
}