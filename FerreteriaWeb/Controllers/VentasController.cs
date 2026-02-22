using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using FerreteriaWeb.Models;

public class VentasController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public VentasController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? "http://localhost:5248/api/");
    }

    public async Task<IActionResult> Create()
    {
        var response = await _httpClient.GetAsync("Productos");
        var productosJson = await response.Content.ReadAsStringAsync();
        ViewBag.ProductosJson = productosJson;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProcesarVenta([FromBody] ProcesarVentaRequest request)
    {
        var venta = new
        {
            cliente = request.Cliente,
            detalles = request.Detalles.Select(d => new { productoId = d.ProductoId, cantidad = d.Cantidad })
        };

        var json = JsonSerializer.Serialize(venta);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("Ventas", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(error);
        }

        var boletaDetalles = new List<object>();
        foreach (var d in request.Detalles)
        {
            var pr = await _httpClient.GetAsync($"Productos/{d.ProductoId}");
            var pJson = await pr.Content.ReadAsStringAsync();
            var p = JsonSerializer.Deserialize<ProductoViewModel>(pJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            boletaDetalles.Add(new
            {
                nombre = p!.Nombre,
                cantidad = d.Cantidad,
                precio = p.Precio,
                subtotal = d.Cantidad * p.Precio
            });
        }

        TempData["Boleta_Cliente"] = request.Cliente;
        TempData["Boleta_Fecha"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        TempData["Boleta_Detalles"] = JsonSerializer.Serialize(boletaDetalles);

        return Ok("/Ventas/Boleta");
    }

    public IActionResult Boleta()
    {
        return View();
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("Ventas");
        if (!response.IsSuccessStatusCode)
            return View(new List<VentaViewModel>());

        var ventasJson = await response.Content.ReadAsStringAsync();
        var ventas = JsonSerializer.Deserialize<List<VentaViewModel>>(ventasJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(ventas ?? new List<VentaViewModel>());
    }
}