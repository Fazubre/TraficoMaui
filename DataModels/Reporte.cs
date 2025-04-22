namespace TraficoCRFront;

public class Reporte
{
    int id { get; set; }
    string mensaje { get; set; }
    float coordenadaX { get; set; }
    float coordenadaY { get; set; }
    bool activo { get; set; }
    int tipoReporte { get; set; }
    int UsuarioId { get; set; }
    int calleId { get; set; }
    int distritoID { get; set; }
}