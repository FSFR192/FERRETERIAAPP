namespace FerreteriaWeb.Models
{
    public class CrearVentaViewModel
    {
        public string Cliente { get; set; } = string.Empty;
        public List<DetalleItemViewModel> Detalles { get; set; } = new List<DetalleItemViewModel>();
    }

    public class DetalleItemViewModel
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class ProcesarVentaRequest
    {
        public string Cliente { get; set; } = string.Empty;
        public List<DetalleItemViewModel> Detalles { get; set; } = new List<DetalleItemViewModel>();
    }
}