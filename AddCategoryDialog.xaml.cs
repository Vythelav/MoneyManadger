using System.Windows;

namespace MoneyManager
{
    public partial class AddCategoryDialog : Window
    {
        public string CategoryName { get; private set; }

        public AddCategoryDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = CategoryNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(CategoryName))
            {
                MessageBox.Show("Пожалуйста, введите название категории.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}