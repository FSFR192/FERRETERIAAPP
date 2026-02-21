using System.Diagnostics.Contracts;

namespace FerreteriaWeb.Models
{
    public class DetalleVentaViewModel
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public ProductoViewModel Producto { get; set; } = new ProductoViewModel();
    }
}