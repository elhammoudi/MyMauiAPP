namespace MyMauiApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();


        string url = "http://intranet/menu/intranetframe.asp";

        // Navigate to the URL in the WebView control
        webView.Source = new UrlWebViewSource { Url = url };

    }

}

