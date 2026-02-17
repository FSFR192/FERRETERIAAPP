namespace FerreteriaAPI
{
    public class ResumenDiaDto
    {
        public DateTimeOffset Fecha { get; set; }
        public int TotalVentas { get; set; }
        public decimal MontoTotal { get; set; }
    }
}