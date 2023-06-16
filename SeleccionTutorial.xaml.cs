using SliptMath.Data;
using SliptMath.Models;
using System.Data;

namespace SliptMath;

public partial class SeleccionTutorial : ContentPage
{
    JugadorDatabase datosJugador;
    //variables para conservar los datos del jugador
    int puntuacionMax;
    int calculadoras;
    Jugador guardarJugador;
    bool BotonPrecionado = false;
    public SeleccionTutorial()
    {
        InitializeComponent();
        datosJugador = new JugadorDatabase();
        obtenerPuntuacion();

    }
    public async void SaltarTutorial(object sender, EventArgs e)
    {
        if(BotonPrecionado == false)
        {
            BotonPrecionado = true;
            guardarJugador = new Jugador { ID = 1, PuntuacionMax = puntuacionMax, CantidadCalculadoras = calculadoras, SaltarTutorial = NoMostrar.IsChecked, MinutosJugar=0,PuntuacionActual= 0 };
            await datosJugador.SaveItemAsync(guardarJugador);
            await Navigation.PushAsync(new MenuModoDeJuego());
            Navigation.RemovePage(this);
        }
    }
    public async void ReproducirTutorial(Object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            guardarJugador = new Jugador { ID = 1, PuntuacionMax = puntuacionMax, CantidadCalculadoras = calculadoras, SaltarTutorial = NoMostrar.IsChecked, MinutosJugar = 0 };
            await datosJugador.SaveItemAsync(guardarJugador);
            await Navigation.PushAsync(new Tutorial());
            Navigation.RemovePage(this);
        }
    }
    public async void obtenerPuntuacion()
    {
        var jugador = await datosJugador.GetItemAsync(1);
        puntuacionMax = jugador.PuntuacionMax;
        calculadoras = jugador.CantidadCalculadoras;
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
            BotonPrecionado = false;
            return true;
        }
        return true;
    }
    void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Reproducir un sonido al cambiar
    }

}
