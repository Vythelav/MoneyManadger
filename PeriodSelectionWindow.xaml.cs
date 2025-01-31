using System;
using System.Windows;

namespace MoneyManager
{
    public partial class PeriodSelectionWindow : Window
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public PeriodSelectionWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = StartDatePicker.SelectedDate ?? DateTime.Now;
            EndDate = EndDatePicker.SelectedDate ?? DateTime.Now;

            DialogResult = true; 
            Close();
        }
    }
}
