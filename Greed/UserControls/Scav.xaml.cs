using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Scav.xaml
    /// </summary>
    public partial class Scav : UserControl
    {
        public Scav()
        {
            InitializeComponent();
        }
        private void HostilitySwitchers(object sender, RoutedEventArgs e)
        {
            if (FriendlyScav.IsChecked == true && HostileScav.IsChecked == true)
            {
                FriendlyScav.IsChecked = false;
            }
        }

        private void FriendlySwitchers(object sender, RoutedEventArgs e)
        {
            if (FriendlyScav.IsChecked == true && HostileScav.IsChecked == true)
            {
                HostileScav.IsChecked = false;
            }
        }

        private void FriendlyBossSwitchers(object sender, RoutedEventArgs e)
        {
            if (FriendlyBoss.IsChecked == true && HostileBoss.IsChecked == true)
            {
                FriendlyBoss.IsChecked = false;
            }
        }
        private void HostilityBossSwitchers(object sender, RoutedEventArgs e)
        {
            if (FriendlyBoss.IsChecked == true && HostileBoss.IsChecked == true)
            {
                HostileBoss.IsChecked = false;
            }
        }
    }
}
