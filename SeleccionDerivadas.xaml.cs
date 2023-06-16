using SliptMath.Data;
using SliptMath.Models;

namespace SliptMath;

public partial class SeleccionDerivadas : ContentPage
{
    bool BotonPrecionado = false;
    JugadorDatabase datosJugador;
    Jugador guardarJugador;
    //Datos Del jugador
    int puntuacionMax;
    int calculadoras;
    bool saltarTutorial;

    public SeleccionDerivadas()
	{
		InitializeComponent();
        datosJugador = new JugadorDatabase();
        obtenerDatos();
	}
    protected override bool OnBackButtonPressed()
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            Dispatcher.Dispatch(async () =>
            {
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            });
            return true;
        }
        return true;
    }
    public async void obtenerDatos()
    {
        var jugador = await datosJugador.GetItemAsync(1);
        puntuacionMax = jugador.PuntuacionMax;
        calculadoras = jugador.CantidadCalculadoras;
        saltarTutorial = jugador.SaltarTutorial;
    }
    public async void Opcion3Minutos(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            guardarJugador = new Jugador { ID = 1, PuntuacionMax = puntuacionMax, CantidadCalculadoras = calculadoras, SaltarTutorial = saltarTutorial, MinutosJugar=3,PuntuacionActual= 0 };
            await datosJugador.SaveItemAsync(guardarJugador);
            await Navigation.PushAsync(new JuegoDerivadas());
            Navigation.RemovePage(this);
        }
    }
    public async void Opcion5Minutos(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            guardarJugador = new Jugador { ID = 1, PuntuacionMax = puntuacionMax, CantidadCalculadoras = calculadoras, SaltarTutorial = saltarTutorial, MinutosJugar = 5,PuntuacionActual= 0 };
            await datosJugador.SaveItemAsync(guardarJugador);
            await Navigation.PushAsync(new JuegoDerivadas());
            Navigation.RemovePage(this);
        }
    }
    public async void Opcion10Minutos(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            guardarJugador = new Jugador { ID = 1, PuntuacionMax = puntuacionMax, CantidadCalculadoras = calculadoras, SaltarTutorial = saltarTutorial, MinutosJugar = 10,PuntuacionActual = 0 };
            await datosJugador.SaveItemAsync(guardarJugador);
            await Navigation.PushAsync(new JuegoDerivadas());
            Navigation.RemovePage(this);
        }
    }
    public async void Opcion15Minutos(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            guardarJugador = new Jugador { ID = 1, PuntuacionMax = puntuacionMax, CantidadCalculadoras = calculadoras, SaltarTutorial = saltarTutorial, MinutosJugar = 15,PuntuacionActual= 0 };
            await datosJugador.SaveItemAsync(guardarJugador);
            await Navigation.PushAsync(new JuegoDerivadas());
            Navigation.RemovePage(this);
        }
    }
}