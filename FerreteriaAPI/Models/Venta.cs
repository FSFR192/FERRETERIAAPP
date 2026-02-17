using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models
{
    public class Venta
    {
        public int Id { get; set; }
        [Required]
        public DateTimeOffset Fecha { get; set; }
        [Required]
        public decimal Total { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}