namespace TraficoCRFront;

public class Reporte
{
    public int id { get; set; }
    public string mensaje { get; set; }
    public float coordenadaX { get; set; }
    public float coordenadaY { get; set; }
    public bool activo { get; set; }
    public int tipoReporte { get; set; }
    public int UsuarioId { get; set; }
    public int calleId { get; set; }
    public int distritoId { get; set; }

    public string? nomDistrito { get; set; }

    public string? nomCanton { get; set; }

    public string? nomProvincia { get; set; }

    public string? nomCalle {  get; set; }
}