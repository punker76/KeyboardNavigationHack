using System.Windows;
using ControlzEx;

namespace KeyboardNavigationHackSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                KeyboardNavigationEx.Focus(this.firstButton);
            };
        }
    }
}
