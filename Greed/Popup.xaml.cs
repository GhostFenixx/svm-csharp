using System.Windows;

namespace Greed
{
    /// <summary>
    /// Логика взаимодействия для Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        public bool Confirm { get; private set; }
        public Popup(string value)
        {
            System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            InitializeComponent();
            textBlock.Text = value;
            this.button.Click += CloseWindow;
        }

        void OnCancel()
        {
            this.Confirm = false;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnOkay(object sender, RoutedEventArgs e)
        {
            this.Confirm = true;
        }
        private void Dragger(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}