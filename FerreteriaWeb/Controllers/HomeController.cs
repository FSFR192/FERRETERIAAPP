using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace FerreteriaWeb.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        var apiUrl = configuration["ApiBaseUrl"] ?? "https://ferreteriaapp-production.up.railway.app/api/";
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(apiUrl);
    }

    public async Task<IActionResult> Index()
    {
        var apiBase = _configuration["ApiBaseUrl"]?.TrimEnd('/')
                          ?? "https://ferreteriaapp-production.up.railway.app/api";
        ViewBag.ApiBaseUrl = apiBase;
        
        try
        {
            var diaResp = await _httpClient.GetAsync("Ventas/resumen-diario");
            var mesResp = await _httpClient.GetAsync("Ventas/Resumen-mensual");

            var diaJson = await diaResp.Content.ReadAsStringAsync();
            var mesJson = await mesResp.Content.ReadAsStringAsync();

            var dia = JsonSerializer.Deserialize<JsonElement>(diaJson);
            var mes = JsonSerializer.Deserialize<JsonElement>(mesJson);

            ViewBag.VentasHoy = dia.GetProperty("totalVentas").GetInt32();
            ViewBag.IngresosHoy = dia.GetProperty("montoTotal").GetDecimal().ToString("0.00");
            ViewBag.VentasMes = mes.GetProperty("totalVentas").GetInt32();
            ViewBag.IngresosMes = mes.GetProperty("montoTotal").GetDecimal().ToString("0.00");
            ViewBag.MesActual = System.Globalization.CultureInfo.CurrentCulture
                .DateTimeFormat.GetMonthName(DateTime.Now.Month);
        }
        catch
        {
            ViewBag.VentasHoy = 0;
            ViewBag.IngresosHoy = "0.00";
            ViewBag.VentasMes = 0;
            ViewBag.IngresosMes = "0.00";
            ViewBag.MesActual = DateTime.Now.ToString("MMMM");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}