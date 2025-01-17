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
using System.Windows.Shapes;

namespace MoneyManadger
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
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
            ManagerFrame.Content = new AccauntPage();
        }

        private void GearImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Content = new SettingPage();
        }
    }
}
