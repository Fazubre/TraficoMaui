using CommunityToolkit.Maui.Views;

namespace TraficoCRFront.Views;

public partial class PopupDetallesReportes : Popup
{
	public PopupDetallesReportes(Reporte reporte)
	{
		InitializeComponent();
        BindingContext = reporte;
	}

    private void Cerrar_Clicked(object sender, EventArgs e)
    {
        this.Close(); 
    }
}