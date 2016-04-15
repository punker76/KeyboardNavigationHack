# KeyboardNavigationHack

Focus an element and shows the focus visual style too! There is maybe a better solution for this, but I don't found it...

**Don't try this @home!**

![button_focus_hack](https://cloud.githubusercontent.com/assets/658431/14565712/a29cdd94-032c-11e6-8454-68bf71a79a62.gif)

Usage

```csharp
private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
{
    KeyboardNavigationHack.Focus(sender as UIElement);
}
```

KeyboardNavigationHack

```csharp
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
```
