using Microsoft.AspNetCore.Mvc;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;
using FerreteriaAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

namespace FerreteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public VentasController(FerreteriaDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] VentaCreateDto ventaDto)
        {
            if (ventaDto.Detalles == null || !ventaDto.Detalles.Any())
                return BadRequest("La venta debe tener al menos un producto.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Venta { 
                    Fecha = DateTime.Now,
                    Total = 0,
                    Cliente = ventaDto.Cliente
                    };
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                foreach (var d in ventaDto.Detalles)
                {
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.Id == d.ProductoId);

                    if (producto == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Producto con ID {d.ProductoId} no existe.");
                    }

                    if (producto.Stock < d.Cantidad)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Stock insuficiente para {producto.Nombre}.");
                    }

                    producto.Stock -= d.Cantidad;

                    var detalleVenta = new DetalleVenta
                    {
                        VentaId = venta.Id,
                        ProductoId = producto.Id,
                        NombreProducto = producto.Nombre, // <- snapshot
                        Cantidad = d.Cantidad,
                        PrecioUnitario = producto.Precio
                    };

                    venta.Total += d.Cantidad * producto.Precio;
                    _context.DetallesVenta.Add(detalleVenta);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Venta registrada exitosamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .Select(v => new VentaDto
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    Detalles = v.Detalles.Select(d => new DetalleVentaDto
                    {
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Producto = new ProductoDto
                        {
                            Id = d.ProductoId ?? 0,
                            Nombre = !string.IsNullOrEmpty(d.NombreProducto)
                                     ? d.NombreProducto
                                     : d.Producto != null ? d.Producto.Nombre : "Producto eliminado",
                            Precio = d.PrecioUnitario
                        }
                    }).ToList()
                }).ToListAsync();

            return Ok(ventas);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .Where(v => v.Id == id)
                .Select(v => new VentaDto
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    Detalles = v.Detalles.Select(d => new DetalleVentaDto
                    {
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Producto = new ProductoDto
                        {
                            Id = d.Producto.Id,
                            Nombre = d.Producto.Nombre,
                            Precio = d.Producto.Precio
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (venta == null)
                return NotFound();

            return Ok(venta);
        }


        [HttpGet("reporte")]

        public async Task<IActionResult> ObtenerReporte(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");

            var ventas = await _context.Ventas
            .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
            .ToListAsync();

            var reporte = new ReporteVentasDto
            {
                TotalVentas = ventas.Count,
                MontoTotal = ventas.Sum(v => v.Total)
            };

            return Ok(reporte);

        }


        [HttpGet("resumen-diario")]
        public async Task<IActionResult> ResumenDia()
        {
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            var ventasHoy = await _context.Ventas
                .Where(v => v.Fecha >= hoy && v.Fecha < mañana)
                .ToListAsync();

            var resumen = new ResumenDiaDto
            {
                Fecha = hoy,
                TotalVentas = ventasHoy.Count,
                MontoTotal = ventasHoy.Sum(v => v.Total)
            };

            return Ok(resumen);
        }

        /*[HttpGet("historial-producto/{productoId}")]
        public async Task<IActionResult> HistorialProducto(int productoId)
        {
            var historial = await _context.DetallesVenta
                .Where(d => d.ProductoId == productoId)
                .Select(d => new HistorialProductoDto
                {
                    Fecha = d.Venta.Fecha,
                    Cantidad = d.Cantidad,
                    Total = d.Cantidad * d.PrecioUnitario
                })
                .ToListAsync();

            return Ok(historial);
        } */

        [HttpGet("Resumen-mensual")]
        public async Task<IActionResult> ResumenMes()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var siguienteMes = inicioMes.AddMonths(1);

            var ventasMes = await _context.Ventas
                .Where(v => v.Fecha >= inicioMes && v.Fecha < siguienteMes)
                .ToListAsync();

            var resultado = new
            {
                Mes = hoy.Month,
                Año = hoy.Year,
                TotalVentas = ventasMes.Count,
                MontoTotal = ventasMes.Sum(v => v.Total)
            };

            return Ok(resultado);
        }

        [HttpGet("dasboard")]
        public async Task<IActionResult> Dashboard()
        {
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var siguienteMes = inicioMes.AddMonths(1);

            var ventasHoy = await _context.Ventas
                .Where(v => v.Fecha >= hoy && v.Fecha < mañana)
                .ToListAsync();

            var ventasMes = await _context.Ventas
                .Where(v => v.Fecha >= inicioMes && v.Fecha < siguienteMes)
                .ToListAsync();

            var dashboard = new DashboardDto
            {
                VentasHoy = ventasHoy.Count,
                IngresosHoy = ventasHoy.Sum(v => v.Total),
                VentasMes = ventasMes.Count,
                IngresosMes = ventasMes.Sum(v => v.Total),
                ProductosStockBajo = await _context.Productos.CountAsync(p => p.Stock <= p.StockMinimo)
            };

            return Ok(dashboard);
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> ExportarVentas(int Mes, int Año)
        {
            var inicioMes = new DateTime(Año, Mes, 1);
            var siguienteMes = inicioMes.AddMonths(1);

            var detalles = await _context.DetallesVenta
                .Include(d => d.Venta)
                .Include(d => d.Producto)
                .Where(d => d.Venta.Fecha >= inicioMes && d.Venta.Fecha < siguienteMes)
                .ToListAsync();

            if (!detalles.Any())
                return BadRequest("No hay ventas en ese mes.");

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ventas");

            // Encabezados
            worksheet.Cell(1, 1).Value = "Fecha";
            worksheet.Cell(1, 2).Value = "N° Venta";
            worksheet.Cell(1, 3).Value = "Producto";
            worksheet.Cell(1, 4).Value = "Cantidad";
            worksheet.Cell(1, 5).Value = "Precio Unitario";
            worksheet.Cell(1, 6).Value = "Total Línea";

            int fila = 2;
            decimal totalGeneral = 0;

            foreach (var d in detalles)
            {
                decimal totalLinea = d.Cantidad * d.PrecioUnitario;
                totalGeneral += totalLinea;

                worksheet.Cell(fila, 1).Value = d.Venta.Fecha.ToString("yyyy-MM-dd");
                worksheet.Cell(fila, 2).Value = d.VentaId;
                worksheet.Cell(fila, 3).Value = d.Producto.Nombre;
                worksheet.Cell(fila, 4).Value = d.Cantidad;
                worksheet.Cell(fila, 5).Value = d.PrecioUnitario;
                worksheet.Cell(fila, 6).Value = totalLinea;

                fila++;
            }

            // Total General
            worksheet.Cell(fila + 1, 5).Value = "TOTAL DEL MES:";
            worksheet.Cell(fila + 1, 6).Value = totalGeneral;

            worksheet.Range(1, 1, 1, 6).Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Ventas_{Mes}_{Año}.xlsx"
            );
        }

    }
}