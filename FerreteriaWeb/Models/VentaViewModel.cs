using System.ComponentModel.DataAnnotations;

namespace FerreteriaWeb.Models
{
public class VentaViewModel
    {
        [Required]
        public int ProductoId { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}