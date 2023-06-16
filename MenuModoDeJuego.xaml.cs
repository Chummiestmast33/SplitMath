using Plugin.Maui.Audio;

namespace SliptMath;

public partial class MenuModoDeJuego : ContentPage
{
    bool AnimacionActiva = true;
    IAudioPlayer player;
    bool AudioActivo = true;
    bool BotonPrecionado = false;
    public MenuModoDeJuego()
    {
        InitializeComponent();
        IniciarReproductor();
        ReproducirAnimaciones();
    }
    protected override bool OnBackButtonPressed()
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            Dispatcher.Dispatch(async () =>
            {
                AnimacionActiva = false;
                player.Stop();
                player.Dispose();
                AnimacionActiva = false;
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            });
            return true;
        }
        return true;
    }
    private async void RegresarMenu(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            AnimacionActiva = false;
            player.Stop();
            player.Dispose();
            AnimacionActiva = false;
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
    private async void IniciarReproductor()
    {
        player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Nintendo_3DS Music_Mii Maker.wav"));
        player.Loop = true;
        player.Play();
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
    private async void ModoDerivadas(object sender, EventArgs e)
    {
        if(BotonPrecionado == false)
        {
            BotonPrecionado = true;
            AnimacionActiva = false;
            player.Stop();
            player.Dispose();
            await Navigation.PushAsync(new SeleccionDerivadas());
            Navigation.RemovePage(this);
        }
    }
    public async void ReproducirAnimaciones()
    {
        while (AnimacionActiva)
        {
            await btnDerivadas.RotateTo(3, 75);
            await btnFactorizacion.RotateTo(-3, 75);
            await btnDerivadas.RotateTo(-3, 75);
            await btnFactorizacion.RotateTo(3, 75);
            await btnFactorizacion.RotateTo(0, 75);
            await btnDerivadas.RotateTo(0, 75);
        }
    }
}