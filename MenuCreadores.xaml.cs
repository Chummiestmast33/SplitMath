using Plugin.Maui.Audio;
using SliptMath.Models;
using SliptMath.ViewModels;

namespace SliptMath;

public partial class MenuCreadores : ContentPage
{
    IAudioPlayer player;
    bool AudioActivo = true;
    bool BotonPrecionado = false;
    public MenuCreadores()
    {
        InitializeComponent();
        var items = new List<Creador>
        {
            new Creador
            {
                Nombre = "Pablo Cortez",
                ImgName = "pablo.jpg",
                Funcion = "Lider de proyecto/programador"
            },

            new Creador
            {
                Nombre = "Lander Medina",
                ImgName = "lander.jpg",
                Funcion = "Diseñador de interfaz/ Diseñador de iconos"
            },

            new Creador
            {
                Nombre = "Kalep Niño",
                ImgName = "kalep.jpg",
                Funcion = "Desarrollador de imagenes"
            },
            new Creador
            {
                Nombre = "David Zamora",
                ImgName = "david.jpg",
                Funcion = "Desarrollador de problemas"
            },
            new Creador
            {
                Nombre = "Diego Salazar",
                ImgName = "diego.jpg",
                Funcion = "Soundtrack Desingner"
            }

        };
        CarouselCreadores.ItemsSource = items;
        IniciarRepodructor();
    }
    private async void IniciarRepodructor()
    {
        player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Nintendo_3DS Music_Mii Maker.wav"));
        player.Loop = true;
        player.Play();
    }
    private void BtnActivarMusica_Clicked(object sender, EventArgs e)
    {
        if (AudioActivo)
        {
            AudioActivo = false;
            player.Pause();
            BtnActivarMusica.Source = "bocinaapagada.png";
        }
        else
        {
            AudioActivo = true;
            player.Play();
            BtnActivarMusica.Source = "bocinaactiva.png";
        }
    }
    protected override bool OnBackButtonPressed()
    {
            Dispatcher.Dispatch(async () =>
            {
                if (BotonPrecionado == false)
                {
                    BotonPrecionado = true;
                    player.Stop();
                    player.Dispose();
                    await Navigation.PushAsync(new MainPage());
                    Navigation.RemovePage(this);
                }
            });
        return true;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if(BotonPrecionado == false)
        {
            BotonPrecionado = true;
            player.Stop();
            player.Dispose();
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
}