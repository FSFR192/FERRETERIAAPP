using System.Collections.Generic;

namespace FerreteriaAPI.DTOs
{
    public class VentaCreateDto
    {
        public string Cliente { get; set; } = "Sin nombre";
        public List<DetalleVentaCreateDto> Detalles { get; set; }
    }
}