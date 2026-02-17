using System;
using System.Collections.Generic;

namespace FerreteriaAPI.DTOs
{
    public class VentaDto
    {
        public int Id { get; set; }
        public DateTimeOffset Fecha { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; }
    }
}