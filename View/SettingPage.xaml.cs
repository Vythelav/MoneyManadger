using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoneyManadger
{
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            App.ApplyTheme(this);
        }

        private void ApplyTheme()
        {
            ToggleThemeButton.Content = App.IsDarkTheme ? "Включить светлую тему" : "Включить темную тему";

            var stackPanel = this.FindName("MainStackPanel") as StackPanel;
            foreach (var child in stackPanel.Children)
            {
                if (child is Button button)
                {
                    button.Background = App.IsDarkTheme
                        ? (SolidColorBrush)Application.Current.Resources["SecondColor"]
                        : (SolidColorBrush)Application.Current.Resources["ElementsColor"];
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ToggleThemeButton_Click(object sender, RoutedEventArgs e)
        {
            
            App.IsDarkTheme = !App.IsDarkTheme;

            foreach (Window window in Application.Current.Windows)
            {
                App.ApplyTheme(window);
            }

            ApplyTheme(); 
        }
    }
}
