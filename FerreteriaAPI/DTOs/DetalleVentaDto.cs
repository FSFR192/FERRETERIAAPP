namespace FerreteriaAPI.DTOs
{
    public class DetalleVentaDto
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public ProductoDto Producto { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; } = new();
     }
}