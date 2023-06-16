using Plugin.Maui.Audio;
using SliptMath.Data;
using SliptMath.Models;

namespace SliptMath;

public partial class MainPage : ContentPage
{
	bool AnimacionActiva = true;
	bool AudioActivo = true;
	bool BotonPresionado = false;
	JugadorDatabase datosJugador;
	IAudioPlayer player;
    public MainPage()
	{
		InitializeComponent();
		datosJugador = new JugadorDatabase();
		CheckUsuario();
		IniciarReproductor();
		ReproducirAnimaciones();
	}
	private async void IniciarReproductor()
	{
		player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Ost_pagina_de_inicio.wav"));
        player.Loop = true;
		player.Play();
	}
	private async void CheckUsuario()
	{
		var jugador = await datosJugador.GetItemAsync(1);
		//si no existe jugador crear un nuevo jugador
		if(jugador == null)
		{
            Jugador jugadorNuevo = new Jugador { PuntuacionMax = 0, CantidadCalculadoras = 0, SaltarTutorial = false, MinutosJugar=0, PuntuacionActual=0 };
			await datosJugador.SaveItemAsync(jugadorNuevo);
        }
    }
	//crear una funcion para detectar el boton 
    protected override bool OnBackButtonPressed()
	{
        if (BotonPresionado == false)
        {
            BotonPresionado = true;
            Dispatcher.Dispatch(async () =>
            {
                bool leave = await DisplayAlert("Estas a punto de salir", "¿Seguro que quieres salir de la aplicacion?", "Si", "No");
                if (leave)
                {
                    System.Environment.Exit(0);
                }
            });
            BotonPresionado = false;
            return true;
        }
        return true;
    }
	public async void IrMenuSeleccion(object sender,EventArgs e)
	{
		if (BotonPresionado == false)
		{
            BotonPresionado = true;
            var preferenciaJugador = await datosJugador.GetItemAsync(1);
            AnimacionActiva = false;
            player.Dispose();
            if (preferenciaJugador.SaltarTutorial)
            {
                await Navigation.PushAsync(new MenuModoDeJuego());
            }
            else
            {
                await Navigation.PushAsync(new SeleccionTutorial());
            }
            Navigation.RemovePage(this);
        }
	}
    public void CambiarReproduccionAudio(object sender, EventArgs e)
    {
        if (AudioActivo)
        {
			AudioActivo = false;
            player.Pause();
            btnSonido.Source = "bocinaapagada.png";
        }
		else
		{
			AudioActivo = true;
			player.Play();
			btnSonido.Source = "bocinaactiva.png";
		}
    }
	public async void ReproducirAnimaciones()
	{
		while (AnimacionActiva)
		{
            await MainImage.ScaleTo(0.8, 90);
            await btnJugar.ScaleTo(0.9, 120);
            await btnPuntuacion.ScaleTo(0.9, 130);
            await MainImage.ScaleTo(1, 100);
            await btnJugar.ScaleTo(1, 100);
            await btnPuntuacion.ScaleTo(1, 100);
        }
	}

    private async void btnPuntuacion_Clicked(object sender, EventArgs e)
    {
        if (BotonPresionado == false)
        {
            BotonPresionado = true;
            AnimacionActiva = false;
            player.Dispose();
            await Navigation.PushAsync(new MenuMejorPuntuacion());
            Navigation.RemovePage(this);
        }
    }

    private async void btnCreadores_Clicked(object sender, EventArgs e)
    {
        if (BotonPresionado == false)
        {
            BotonPresionado = true;
            AnimacionActiva = false;
            player.Dispose();
            await Navigation.PushAsync(new MenuCreadores());
            Navigation.RemovePage(this);
        }
    }
}

