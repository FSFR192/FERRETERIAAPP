namespace FerreteriaAPI.DTOs
{
    public class DashboardDto
    {
        public int VentasHoy { get; set; }
        public decimal IngresosHoy { get; set; }
        public int VentasMes { get; set; }
        public decimal IngresosMes { get; set; }
        public int ProductosStockBajo { get; set; }
    }
}