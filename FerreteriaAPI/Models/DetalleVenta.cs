using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreteriaAPI.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        [Required]
        public int VentaId { get; set; }
        public int? ProductoId { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("VentaId")]
        public Venta Venta { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }
        public string NombreProducto { get; set; } // <- guardar snapshot
    }
}