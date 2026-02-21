using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FerreteriaWeb.Models
{
    public class VentaViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaViewModel> Detalles { get; set; } = new List<DetalleVentaViewModel>();
    }

    // Para el formulario crear
    public class CrearVentaViewModel
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}