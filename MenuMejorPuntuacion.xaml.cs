using Plugin.Maui.Audio;
using SliptMath.Data;
using SliptMath.Models;

namespace SliptMath;

public partial class MenuMejorPuntuacion : ContentPage
{
    JugadorDatabase datosJugador;
    bool BotonPrecionado;
    bool animar = true;
    IAudioPlayer player;
    public MenuMejorPuntuacion()
    {
        InitializeComponent();
        datosJugador = new JugadorDatabase();
        CargarPuntuacion();
        ReproducirMusica();
        CambiarDeColores();
    }
    private async void CargarPuntuacion()
    {
        Jugador jugador = await datosJugador.GetItemAsync(1);
        LbMejorPuntuacion.Text = Convert.ToString(jugador.PuntuacionMax);
    }
    private async void ReproducirMusica()
    {
        player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("musica_puntuacion.mp3"));
        player.Loop = true;
        player.Play();
    }
    public async void btnVolverMenu_Clicked(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            animar = false;
            player.Stop();
            player.Dispose();
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
    protected override bool OnBackButtonPressed()
    {

        Dispatcher.Dispatch(async () =>
        {
            if (BotonPrecionado == false)
            {
                BotonPrecionado = true;
                animar = false;
                player.Stop();
                player.Dispose();
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
            BotonPrecionado = false;
        });
        return true;
    }
    public async void CambiarDeColores()
    {
        while (animar)
        {
            await btnVolverMenu.ScaleTo(0.8);
            LbMejorPuntuacion.TextColor = Color.FromRgb(217, 37, 101);
            await Task.Delay(200);
            await btnVolverMenu.ScaleTo(1.1);
            LbMejorPuntuacion.TextColor = Color.FromRgb(119, 193, 121);
            await Task.Delay(200);
            await btnVolverMenu.ScaleTo(0.9);
            LbMejorPuntuacion.TextColor = Color.FromRgb(43, 34, 133);
            await Task.Delay(200);
            await btnVolverMenu.ScaleTo(0.7);
            LbMejorPuntuacion.TextColor = Color.FromRgb(242, 172, 61);
            await Task.Delay(200);
            await btnVolverMenu.ScaleTo(1);
            LbMejorPuntuacion.TextColor = Color.FromRgb(17, 246, 160);
        }

    }
}