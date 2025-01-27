using System.Windows;

namespace MoneyManadger
{
    public partial class NameInputDialog : Window
    {
        public string NewName { get; private set; }

        public NameInputDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewName = NameTextBox.Text;
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
