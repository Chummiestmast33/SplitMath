namespace SliptMath;

public partial class Tutorial : ContentPage
{
    bool BotonPrecionado = false;
    byte clickContador = 0;
	public Tutorial()
	{
		InitializeComponent();
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

    private async void BotonSiguiente_Clicked(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            switch (clickContador)
            {
                case 0:
                    ImagenRepresentacion.Source = "escoje_un_tema.jpg";
                    Descripcion.Text = "Primero, elige el modo de juego. Por ahora solo esta disponible el modo de derivadas.";
                    break;
                case 1:
                    ImagenRepresentacion.Source = "seleccion_tiempo.jpg";
                    Descripcion.Text = "Ahora elije el tiempo que quieres jugar. Si sales antes de que se termine el tiempo no se guardara tu puntuacion.";
                    break;
                case 2:
                    ImagenRepresentacion.Source = "resolver_problema.jpg";
                    Descripcion.Text = "Muy bien. Ahora comienza el juego. Recuerda que, a pesar de estar contrareloj, tienes tiempo para pensar en la respuesta y elejirla";
                    break;
                case 3:
                    ImagenRepresentacion.Source = "uso_de_calculadora.jpg";
                    Descripcion.Text = "Si logras resolver 5 problemas seguidos, conseguiras una calculadora que cuando la precionas te muestra la respuesta correcta";
                    break;
                case 4:
                    ImagenRepresentacion.Source = "resolver_problema.jpg";
                    Descripcion.Text = "Estos son los basicos del juego. Gracias por instalarlo y recuerda que si respondes incorrectamente perderas 10 puntos. !Buena suerte¡";
                    break;
                case 5:
                    await Navigation.PushAsync(new MenuModoDeJuego());
                    Navigation.RemovePage(this);
                    return;
            }
            clickContador++;
            BotonPrecionado = false;
        }
    }

    private async void BotonRegresar_Clicked(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
}