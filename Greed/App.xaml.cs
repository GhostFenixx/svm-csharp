using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Greed
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
        }
    }

}
