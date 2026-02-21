namespace FerreteriaAPI.DTOs
{
    public class DetalleVentaDto
    {
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public ProductoDto Producto { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; } = new();
     }
}