using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Quests.xaml
    /// </summary>
    /// 
    public partial class Quests : UserControl
    {
        public Quests()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = MainWindow.NumberValidationRegex();
            e.Handled = regex.IsMatch(e.Text);
        }

    }

}
