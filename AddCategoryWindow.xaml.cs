using System.Windows;

namespace MoneyManager
{
    public partial class AddCategoryWindow : Window
    {
        public string CategoryName { get; private set; }

        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                CategoryName = CategoryNameTextBox.Text;
                DialogResult = true; 
                Close(); 
            }
            else
            {
                MessageBox.Show("Введите корректное название категории.");
            }
        }
    }
}
