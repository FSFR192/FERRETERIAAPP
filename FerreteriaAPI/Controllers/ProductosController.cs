using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;
using FerreteriaAPI.DTOs;

namespace FerreteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public ProductosController(FerreteriaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpGet("stock-bajo")]
        public async Task<IActionResult> ProductosStockBajo()
        {
            var productos = await _context.Productos
                .Where(p => p.Stock <= p.StockMinimo )
                .Select(p => new ProductosStockBajoDto
                {
                id = p.Id,
                Nombre = p.Nombre,
                Stock = p.Stock
            })
            .ToListAsync();

            return Ok(productos);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarProducto([FromQuery] string nombre)
        {
            var productos = await _context.Productos
                .Where(p => p.Nombre.Contains(nombre))
                .Select(p => new 
                {
                    p.Id,
                    p.Nombre,
                    p.Precio,
                    p.Stock
                })
                .ToListAsync();

            return Ok(productos);
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            var existe = await _context.Productos
                .AnyAsync(p => p.Nombre.ToLower() == producto.Nombre.ToLower());
            if (existe)
            {
                return BadRequest($"El producto '{producto.Nombre}' ya existe.");
            }
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
