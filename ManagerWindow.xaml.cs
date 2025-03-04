﻿using MoneyManager;
using System;
using System.Windows;
using System.Windows.Input;

namespace MoneyManadger
{
    public partial class ManagerWindow : Window
    {
        private User currentUser; 

        public ManagerWindow(User user) 
        {
            InitializeComponent();
            currentUser = user; 
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
