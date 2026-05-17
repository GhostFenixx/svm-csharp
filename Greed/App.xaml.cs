using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Greed
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Removed default startup uri and do try-catch so i could handle some errors outside the usual scope i can predict, handy for debugging and troubleshooting, no need for going to event viewer.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            try
            {
                var dict = new ResourceDictionary(); //Unusual workaround in case of broken dictionaries, removing startup from app.xaml, declare main one in try-catch, then catch it in case something went south.

                dict.Source = new Uri(
                    "/Languages/English/Dictionary-en-US.xaml",
                    UriKind.Relative);
                Resources.MergedDictionaries.Add(dict);
                MainWindow window = new MainWindow();
                window.Show();
            }
            catch (Exception ex)
            {
                Popup message = new("Application have crashed, that shouldn't have happened.\nReport in Fika/SPT mod-questions\n" + ex);
                message.ShowDialog();
                    Shutdown();
            }
        }
    }
}
