using Plugin.Maui.Audio;
using SliptMath.Data;
using SliptMath.Models;

namespace SliptMath;

public partial class JuegoDerivadas : ContentPage
{
    //reproductores
    IAudioPlayer sonidoCorrecto;
    IAudioPlayer sonidoIncorrecto;
    //modelos y base de datos
    Jugador DatosActuales;
    JugadorDatabase DatosGuardados;
    //variables para aguarda datos
    int puntuacionActual = 0;
    int calculadorasActuales;
    int PuntuacionMaxima;
    bool SaltarTutorialOpcion;
    //variables de tiempo
    int MinutosJugar;
    int tiempoTotal;
    int tiempoActualSegundos;
    bool BotonPrecionado = false;
    Timer timer;
    bool elTiempoEstaActivo;
    //generacion de problemas
    private List<ProblemaDerivacion> problemas = new List<ProblemaDerivacion>();
    ProblemaDerivacion problemaActual;
    int respuestasSeguidas = 0;
    bool CalculadoraUsada = false;
    bool respuestaObtenida = false;
    //colores
    Color verde = Color.FromRgba(102, 255, 51, 255);
    Color rojo = Color.FromRgba(255, 80, 80, 255);
    public JuegoDerivadas()
    {
        InitializeComponent();
        DatosGuardados = new JugadorDatabase();
        GenerarProblemas();
        ObtenerDatos();
        CargarNuevoProblema();

    }
    private void crearTemporizador()
    {
        timer = new Timer(TimerCallback, null, 0, 1000);
    }
    private async void CrearReproductores()
    {
        sonidoCorrecto = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("respuesta_correcta.mp3"));
        sonidoIncorrecto = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("respuesta_incorrecta.mp3"));
    }
    private void TimerCallback(object state)
    {
        if (!elTiempoEstaActivo) return;

        tiempoActualSegundos--;

        // Actualiza el texto del Label en el hilo principal
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            timerLabel.Text = TimeSpan.FromSeconds(tiempoActualSegundos).ToString(@"mm\:ss");

            if (tiempoActualSegundos <= 0)
            {
                elTiempoEstaActivo = false;
                timer.Dispose();
                if (puntuacionActual > PuntuacionMaxima)
                {
                    Jugador JugadorDatosNuevos = new Jugador { ID = 1, PuntuacionMax = puntuacionActual, CantidadCalculadoras = calculadorasActuales, SaltarTutorial = SaltarTutorialOpcion, MinutosJugar = 0, PuntuacionActual = puntuacionActual };
                    await DatosGuardados.SaveItemAsync(JugadorDatosNuevos);
                }
                else
                {
                    Jugador JugadorDatosNuevos = new Jugador { ID = 1, PuntuacionMax = PuntuacionMaxima, CantidadCalculadoras = calculadorasActuales, SaltarTutorial = SaltarTutorialOpcion, MinutosJugar = 0, PuntuacionActual = puntuacionActual };
                    await DatosGuardados.SaveItemAsync(JugadorDatosNuevos);
                }
                await Navigation.PushAsync(new PaginaDePuntuacion());
                Navigation.RemovePage(this);
                // Aquí puedes realizar la navegación a otra página
            }
        });
    }

    protected override bool OnBackButtonPressed()
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            Dispatcher.Dispatch(async () =>
            {
                bool Salir = await DisplayAlert("Estas apunto de salir", "Si sales perderas todo el progreso ¿Estas seguro?", "si", "No");
                if (Salir == true)
                {
                    sonidoCorrecto.Dispose();
                    sonidoIncorrecto.Dispose();
                    await Navigation.PushAsync(new MenuModoDeJuego());
                    Navigation.RemovePage(this);
                }
                BotonPrecionado = false;

            });
            return true;
        }
        return true;
    }
    private void GenerarProblemas()
    {
        Random numRandom = new();
        for (byte i = 1; i <= 40; i++)
        {
            int indiceRandom = numRandom.Next(1, 4);
            ProblemaDerivacion problema = new() { EquationImage = $"equation{i}.png", CorrectAnswerImage = $"correct{i}.png", IncorrectAnswer1Image = $"incorrect{i}_1.png", IncorrectAnswer2Image = $"incorrect{i}_2.png", IndiceCorrecto = indiceRandom };
            problemas.Add(problema);
        }
    }
    private void CargarNuevoProblema()
    {
        problemaActual = ObtenerProblemaRandom();
        switch (problemaActual.IndiceCorrecto)
        {
            case 1:
                imgEcuacion.Source = problemaActual.EquationImage;
                btnUno.Source = problemaActual.CorrectAnswerImage;
                btnDos.Source = problemaActual.IncorrectAnswer1Image;
                btnTres.Source = problemaActual.IncorrectAnswer2Image;
                break;
            case 2:
                imgEcuacion.Source = problemaActual.EquationImage;
                btnUno.Source = problemaActual.IncorrectAnswer1Image;
                btnDos.Source = problemaActual.CorrectAnswerImage;
                btnTres.Source = problemaActual.IncorrectAnswer2Image;
                break;
            case 3:
                imgEcuacion.Source = problemaActual.EquationImage;
                btnUno.Source = problemaActual.IncorrectAnswer1Image;
                btnDos.Source = problemaActual.IncorrectAnswer2Image;
                btnTres.Source = problemaActual.CorrectAnswerImage;
                break;
        }
    }
    private ProblemaDerivacion ObtenerProblemaRandom()
    {
        Random numRandom = new();
        int RandomIndex = numRandom.Next(problemas.Count);
        return problemas[RandomIndex];
    }
    private async void ComprobarRespuesta(int numBoton)
    {
        CalculadoraUsada = false;
        switch (problemaActual.IndiceCorrecto)
        {
            case 1:
                if (numBoton == 1)
                {
                    //agregar animaciones
                    puntuacionActual += 10;
                    respuestasSeguidas += 1;
                    LbPuntaje.Text = $"Puntuacion:{puntuacionActual}";
                    sonidoCorrecto.Play();
                    ColorearBtn1();
                    await Task.Delay(1350);
                    AnadirCalculadoras();
                    CargarNuevoProblema();
                }
                else
                {
                    if (puntuacionActual > 0)
                    {
                        puntuacionActual -= 10;
                        LbPuntaje.Text = $"Puntuacion:{puntuacionActual}";
                    }
                    respuestasSeguidas = 0;
                    sonidoIncorrecto.Play();
                    ColorearBtn1();
                    await Task.Delay(1350);
                    CargarNuevoProblema();
                }
                break;
            case 2:
                if (numBoton == 2)
                {
                    //agregar animaciones
                    puntuacionActual += 10;
                    respuestasSeguidas += 1;
                    LbPuntaje.Text = $"Puntuacion:{puntuacionActual}";
                    sonidoCorrecto.Play();
                    ColorearBtn2();
                    await Task.Delay(1350);
                    AnadirCalculadoras();
                    CargarNuevoProblema();
                }
                else
                {
                    if (puntuacionActual > 0)
                    {
                        puntuacionActual -= 10;
                        LbPuntaje.Text = $"Puntuacion:{puntuacionActual}";
                    }
                    respuestasSeguidas = 0;
                    sonidoIncorrecto.Play();
                    ColorearBtn2();
                    await Task.Delay(1350);
                    CargarNuevoProblema();
                }
                break;
            case 3:
                if (numBoton == 3)
                {
                    //agregar animaciones
                    puntuacionActual += 10;
                    respuestasSeguidas += 1;
                    LbPuntaje.Text = $"Puntuacion:{puntuacionActual}";
                    sonidoCorrecto.Play();
                    ColorearBtn3();
                    await Task.Delay(1350);
                    AnadirCalculadoras();
                    CargarNuevoProblema();
                }
                else
                {
                    if (puntuacionActual > 0)
                    {
                        puntuacionActual -= 10;
                        LbPuntaje.Text = $"Puntuacion:{puntuacionActual}";
                    }
                    respuestasSeguidas = 0;
                    sonidoIncorrecto.Play();
                    ColorearBtn3();
                    await Task.Delay(1350);
                    CargarNuevoProblema();
                }
                break;
        }
        respuestaObtenida = false;
    }
    private void BtnRespuesta1(object sender, EventArgs e)
    {
        if (respuestaObtenida == false)
        {
            respuestaObtenida = true;
            ComprobarRespuesta(1);
        }
    }
    private void BtnRespuesta2(object sender, EventArgs e)
    {
        if (respuestaObtenida == false)
        {
            respuestaObtenida = true;
            ComprobarRespuesta(2);
        }
    }
    private void BtnRespuesta3(object sender, EventArgs e)
    {
        if (respuestaObtenida == false)
        {
            respuestaObtenida = true;
            ComprobarRespuesta(3);
        }
    }
    public void BtnCalculadora(object sender, EventArgs e)
    {
        if (CalculadoraUsada == false)
        {
            CalculadoraUsada = true;
            RemoverCalculadora();
            switch (problemaActual.IndiceCorrecto)
            {
                case 1:
                    SenalarBtn1();
                    break;
                case 2:
                    SenalarBtn2();
                    break;
                case 3:
                    SenalarBtn3();
                    break;
            }
        }
    }
    #region metodos BotonesColorear
    public async void ColorearBtn1()
    {
        btnUno.BackgroundColor = verde;
        btnDos.BackgroundColor = rojo;
        btnTres.BackgroundColor = rojo;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        await Task.Delay(350);
        btnUno.BackgroundColor = verde;
        btnDos.BackgroundColor = rojo;
        btnTres.BackgroundColor = rojo;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
    }

    public async void ColorearBtn2()
    {
        btnUno.BackgroundColor = rojo;
        btnDos.BackgroundColor = verde;
        btnTres.BackgroundColor = rojo;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        await Task.Delay(350);
        btnUno.BackgroundColor = rojo;
        btnDos.BackgroundColor = verde;
        btnTres.BackgroundColor = rojo;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
    }

    public async void ColorearBtn3()
    {
        btnUno.BackgroundColor = rojo;
        btnDos.BackgroundColor = rojo;
        btnTres.BackgroundColor = verde;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        await Task.Delay(350);
        btnUno.BackgroundColor = rojo;
        btnDos.BackgroundColor = rojo;
        btnTres.BackgroundColor = verde;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
    }
    public async void SenalarBtn1()
    {
        btnUno.BackgroundColor = verde;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        await Task.Delay(350);
        btnUno.BackgroundColor = verde;
        await Task.Delay(350);
        btnUno.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
    }

    public async void SenalarBtn2()
    {
        btnDos.BackgroundColor = verde;
        await Task.Delay(350);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        await Task.Delay(350);
        btnDos.BackgroundColor = verde;
        await Task.Delay(350);
        btnDos.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
    }
    public async void SenalarBtn3()
    {
        btnTres.BackgroundColor = verde;
        await Task.Delay(350);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        await Task.Delay(350);
        btnTres.BackgroundColor = verde;
        await Task.Delay(350);
        btnTres.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
    }
    #endregion
    private async void AnadirCalculadoras()
    {
        if (respuestasSeguidas == 5)
        {
            respuestasSeguidas = 0;
            calculadorasActuales += 1;
            Jugador JugadorDatosNuevos = new Jugador { ID = 1, PuntuacionMax = PuntuacionMaxima, CantidadCalculadoras = calculadorasActuales, SaltarTutorial = SaltarTutorialOpcion, MinutosJugar = 0, PuntuacionActual = puntuacionActual };
            await DatosGuardados.SaveItemAsync(JugadorDatosNuevos);
            HabilitarCalculadora();
        }
    }
    private async void RemoverCalculadora()
    {
        calculadorasActuales -= 1;
        Jugador JugadorDatosNuevos = new Jugador { ID = 1, PuntuacionMax = PuntuacionMaxima, CantidadCalculadoras = calculadorasActuales, SaltarTutorial = SaltarTutorialOpcion, MinutosJugar = 0, PuntuacionActual = puntuacionActual };
        HabilitarCalculadora();
        await DatosGuardados.SaveItemAsync(JugadorDatosNuevos);
    }
    private void HabilitarCalculadora()
    {
        if (calculadorasActuales > 0)
        {
            Calculadora.Opacity = 1;
            Calculadora.IsEnabled = true;
        }
        else
        {
            Calculadora.Opacity = 0.7;
            Calculadora.IsEnabled = false;
        }
        LbCantidadCalculadoras.Text = $"calcus: {calculadorasActuales}";
    }
    private async void ObtenerDatos()
    {
        DatosActuales = await DatosGuardados.GetItemAsync(1);
        calculadorasActuales = DatosActuales.CantidadCalculadoras;
        PuntuacionMaxima = DatosActuales.PuntuacionMax;
        SaltarTutorialOpcion = DatosActuales.SaltarTutorial;
        MinutosJugar = DatosActuales.MinutosJugar;
        //creacion de el tiempo y chequeo de cantidad de calculadoras aqui por problemas de la declaracion de varibles con tareas asincronas(descubrir por que)
        HabilitarCalculadora();
        tiempoTotal = MinutosJugar * 60;
        tiempoActualSegundos = tiempoTotal;
        elTiempoEstaActivo = true;
        crearTemporizador();
        CrearReproductores();
    }

    private async void btnVolver_Clicked(object sender, EventArgs e)
    {
        if (BotonPrecionado == false)
        {
            BotonPrecionado = true;
            bool Salir = await DisplayAlert("Estas apunto de salir", "Si sales perderas todo el progreso ¿Estas seguro?", "si", "No");
            if (Salir == true)
            {
                sonidoIncorrecto.Dispose();
                sonidoCorrecto.Dispose();
                await Navigation.PushAsync(new MenuModoDeJuego());
                Navigation.RemovePage(this);
                return;
            }
            BotonPrecionado = false;
        }
    }
}