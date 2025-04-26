using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
namespace TraficoCRFront.Views;

public partial class PopupDetallesReportes : Popup
{
	public PopupDetallesReportes(Reporte reporte)
	{
		InitializeComponent();
        BindingContext = reporte;
        AgregarPin();
        asignarTipoReporte();

    }

	private void AgregarPin()
	{
		try
		{
			// Obtener coordenadas del BindingContext
			var vm = BindingContext;
			var tipo = vm.GetType();

			// Usa reflexión para obtener los valores
			double lat = Convert.ToDouble(tipo.GetProperty("coordenadaY")?.GetValue(vm));
			double lon = Convert.ToDouble(tipo.GetProperty("coordenadaX")?.GetValue(vm));

			var ubicacion = new Location(lat, lon);
			var pin = new Pin
			{
				Label = "Ubicación del reporte",
				Location = ubicacion,
				Type = PinType.Place
			};

			MapPop.Pins.Add(pin);
			MapPop.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacion, Distance.FromKilometers(0.5)));
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error al agregar el pin: {ex.Message}");
		}
	}

    private void asignarTipoReporte()
    {
        var vm = BindingContext;
        var tipo = vm.GetType();

		int tipoReporte = Convert.ToInt32(tipo.GetProperty("tipoReporte")?.GetValue (vm));

		switch (tipoReporte)
		{
			case 1: 
				lblTipoReporte.Text = "Hueco"; 
				break;
            case 2:
                lblTipoReporte.Text = "Calle Bloqueada";
                break;
            case 3:
                lblTipoReporte.Text = "Deslizamiento";
                break;
            case 4:
                lblTipoReporte.Text = "Inundación";
                break;
            case 5:
                lblTipoReporte.Text = "Infraestructura Peligrosa";
                break;
            case 6:
                lblTipoReporte.Text = "Otro";
                break;
            case 7:
                lblTipoReporte.Text = "Caballo Muerto Bloqueando Calle";
                break;
            case 8:
                lblTipoReporte.Text = "Vehiculo Bloqueando Calle";
                break;
        }
    }

    private void Cerrar_Clicked(object sender, EventArgs e)
    {
        this.Close(); 
    }
}