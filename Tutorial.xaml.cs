namespace SliptMath;

public partial class Tutorial : ContentPage
{
    bool BotonPrecionado = false;
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
}