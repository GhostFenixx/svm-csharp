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
    /// Interaction logic for Items.xaml
    /// </summary>
    public partial class Items : UserControl
    {
        public Items()
        {
            InitializeComponent();
        }
        private void CurrencyAmmoExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (AmmoExpander is not null && CurrencyExpander is not null)
            {
                AmmoExpander.IsExpanded = false;
                CurrencyExpander.IsExpanded = false;
            }
        }

        private void CurrencyAmmoExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (AmmoExpander is not null && CurrencyExpander is not null)
            {
                AmmoExpander.IsExpanded = true;
                CurrencyExpander.IsExpanded = true;
            }
        }
    }
}
