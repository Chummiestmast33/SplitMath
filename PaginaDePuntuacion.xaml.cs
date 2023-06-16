using SliptMath.Data;
using SliptMath.Models;

namespace SliptMath;

public partial class PaginaDePuntuacion : ContentPage
{   bool BotonPresionado = false;
    int PuntuacionMaxima;
    int CantidadDeCalculadorasAc;
    bool SaltarTutorial;
    int puntuacionActual;
    JugadorDatabase jugadorDatabase;
	public PaginaDePuntuacion()
	{
		InitializeComponent();
        obtenerDatosAsignar();
	}
    protected override bool OnBackButtonPressed()
    {
        if (BotonPresionado == false)
        {
            BotonPresionado = true;
            Dispatcher.Dispatch(async () =>
            {
                Jugador jugador = new Jugador { ID = 1,PuntuacionMax = PuntuacionMaxima,SaltarTutorial=SaltarTutorial,PuntuacionActual = 0,CantidadCalculadoras=CantidadDeCalculadorasAc,MinutosJugar = 0 };
                await jugadorDatabase.SaveItemAsync(jugador);
                await Navigation.PushAsync(new MenuModoDeJuego());
                Navigation.RemovePage(this);
            });
            BotonPresionado = false;
            return true;
        }
        return true;
    }
    private async void obtenerDatosAsignar()
    {
        jugadorDatabase = new JugadorDatabase();
        Jugador jugador = await jugadorDatabase.GetItemAsync(1);
        PuntuacionMaxima = jugador.PuntuacionMax;
        CantidadDeCalculadorasAc = jugador.CantidadCalculadoras;
        SaltarTutorial = jugador.SaltarTutorial;
        puntuacionActual = jugador.PuntuacionActual;
        LbMejorPuntuacion.Text = Convert.ToString(PuntuacionMaxima);
        LbPuntuacionActual.Text = Convert.ToString(puntuacionActual);
    }
    private async void btnVolverMenu_Clicked(object sender, EventArgs e)
    {
        if(BotonPresionado == false)
        {
            BotonPresionado = true;
            Jugador jugador = new Jugador { ID = 1, PuntuacionMax = PuntuacionMaxima, SaltarTutorial = SaltarTutorial, PuntuacionActual = 0, CantidadCalculadoras = CantidadDeCalculadorasAc, MinutosJugar = 0 };
            await jugadorDatabase.SaveItemAsync(jugador);
            await Navigation.PushAsync(new MenuModoDeJuego());
            Navigation.RemovePage(this);
        }
    }
}