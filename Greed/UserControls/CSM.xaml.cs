using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Greed.UserControls
{
    /// <summary>
    /// Interaction logic for CSM.xaml
    /// </summary>
    public partial class CSM : UserControl
    {
        public CSM()
        {
            InitializeComponent();
        }
        private void InstallPlugin(object sender, RoutedEventArgs e)
        {
            string bepinexFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "BepInEx", "plugins");
            string pluginname = "HideSpecialIconGrids.dll";
            try
            {
                if (!File.Exists(System.IO.Path.Combine(bepinexFolder, pluginname)))
                {
                    Stream stream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Greed.Resources.HideSpecialIconGrids.dll");
                    var fileStream = File.Create(System.IO.Path.Combine(bepinexFolder, pluginname));
                    stream2.CopyTo(fileStream);
                    fileStream.Close();
                    Popup Message = new((string)Application.Current.FindResource("InstallPluginComplete"));
                    Message.ShowDialog();
                }
                else
                {
                    Popup Message = new((string)Application.Current.FindResource("InstallationAlreadyDone"));
                    Message.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Popup Message = new((string)Application.Current.FindResource("InstallPluginFailed") + "\n\n" + ex);
                Message.ShowDialog();
            }
        }
    }
}
