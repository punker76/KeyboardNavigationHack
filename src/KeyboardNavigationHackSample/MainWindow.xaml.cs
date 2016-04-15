using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

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
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            KeyboardNavigationHack.Focus(sender as UIElement);
        }

        public sealed class KeyboardNavigationHack : IDisposable
        {
            //internal static bool AlwaysShowFocusVisual
            private PropertyInfo alwaysShowFocusVisual;
            //internal static void ShowFocusVisual()
            private MethodInfo showFocusVisual;

            public KeyboardNavigationHack()
            {
                var type = typeof (KeyboardNavigation);
                this.alwaysShowFocusVisual = type.GetProperty("AlwaysShowFocusVisual", BindingFlags.NonPublic | BindingFlags.Static);
                this.showFocusVisual = type.GetMethod("ShowFocusVisual", BindingFlags.NonPublic | BindingFlags.Static);
            }

            public void ShowFocusVisual()
            {
                this.showFocusVisual.Invoke(null, null);
            }

            public bool AlwaysShowFocusVisual
            {
                get { return (bool) this.alwaysShowFocusVisual.GetValue(null, null); }
                set { this.alwaysShowFocusVisual.SetValue(null, value, null); }
            }

            public void Dispose()
            {
                this.alwaysShowFocusVisual = null;
                this.showFocusVisual = null;
            }

            /// <summary>
            /// Focuses the specified element and shows the focus visual style.
            /// </summary>
            /// <param name="element">The element.</param>
            public static void Focus(UIElement element)
            {
                element?.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    using (var keybHack = new KeyboardNavigationHack())
                    {
                        var alwaysShowFocusVisual = keybHack.AlwaysShowFocusVisual;
                        keybHack.AlwaysShowFocusVisual = true;
                        try
                        {
                            Keyboard.Focus(element);
                            keybHack.ShowFocusVisual();
                        }
                        finally
                        {
                            keybHack.AlwaysShowFocusVisual = alwaysShowFocusVisual;
                        }
                    }
                }));
            }
        }
    }
}
