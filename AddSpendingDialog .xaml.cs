using System;
using System.Collections.Generic;
using System.Windows;

namespace MoneyManadger
{
    public partial class AddSpendingDialog : Window
    {
        public int Amount { get; private set; }
        public string Category { get; private set; }

        public AddSpendingDialog(IEnumerable<string> categoryList)
        {
            InitializeComponent();
            CategoryComboBox.ItemsSource = categoryList;
            if (CategoryComboBox.Items.Count > 0)
                CategoryComboBox.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AmountTextBox.Text, out int amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Amount = amount;
            Category = CategoryComboBox.SelectedItem.ToString();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}