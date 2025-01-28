using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoneyManadger
{
    public partial class App : Application
    {
        public static bool IsDarkTheme { get; set; } = false;

        public static void ApplyTheme(FrameworkElement element)
        {
            if (element is Control control)
            {
                if (IsDarkTheme)
                {
                    control.Background = (SolidColorBrush)Application.Current.Resources["BlackTeam"];
                    control.Foreground = Brushes.White; 
                }
                else
                {
                    control.Background = (SolidColorBrush)Application.Current.Resources["PrimaryColor"];
                    control.Foreground = Brushes.Black; 
                }

                if (control.Style != null)
                {
                    control.ClearValue(Control.ForegroundProperty);
                }
            }

            if (element is Page page)
            {
                ApplyThemeToChildren(page);
            }
        }

        private static void ApplyThemeToChildren(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement frameworkElement)
                {
                    ApplyTheme(frameworkElement);
                }
                ApplyThemeToChildren(child);
            }
        }
    }
}
