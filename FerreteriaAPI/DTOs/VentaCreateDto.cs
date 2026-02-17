using System.Collections.Generic;

namespace FerreteriaAPI.DTOs
{
    public class VentaCreateDto
    {
        public List<DetalleVentaCreateDto> Detalles { get; set; }
    }
}