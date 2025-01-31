using System;
using System.Windows;

namespace MoneyManadger
{
    public partial class AddSpendingDialog : Window
    {
        public double Amount { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }

        public AddSpendingDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            double amount;

            if (!double.TryParse(AmountTextBox.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Amount = amount;

            Category = CategoryTextBox.Text;
            Description = SpendingAmountTextBox.Text;

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
