using System;
using System.Windows;
using System.Windows.Input;

namespace MoneyManadger
{
    public partial class ManagerWindow : Window
    {
        private User currentUser; // Добавьте поле для текущего пользователя

        public ManagerWindow(User user) // Измените конструктор для принятия объекта User
        {
            InitializeComponent();
            currentUser = user; // Сохраните текущего пользователя
            ManagerFrame.Content = new HomePage();
        }

        private void HomeImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Content = new HomePage();
        }

        private void WalletImageClik(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Content = new IncomePage();
        }

        private void SpendingImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Content = new SpendingPage();
        }

        private void UserImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Content = new AccauntPage(currentUser); 
        }

        private void GearImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Content = new SettingPage();
        }
    }
}
