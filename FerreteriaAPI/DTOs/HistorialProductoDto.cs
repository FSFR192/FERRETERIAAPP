namespace FerreteriaAPI
{
    public class HistorialProductoDto
    {
        public DateTimeOffset Fecha { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}