using System;
using System.Windows;

namespace MoneyManager
{
    public partial class AddIncomeWindow : Window
    {
        public string Category { get; private set; }
        public decimal Amount { get; private set; }

        public AddIncomeWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && !string.IsNullOrWhiteSpace(CategoryTextBox.Text))
            {
                Category = CategoryTextBox.Text;
                Amount = amount;
                DialogResult = true; 
                Close(); 
            }
            else
            {
                MessageBox.Show("Введите корректные данные о доходе.");
            }
        }
    }
}
