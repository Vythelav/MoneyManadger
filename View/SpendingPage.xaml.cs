using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace MoneyManadger
{
    public partial class SpendingPage : Page
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=MoneyManager;Trusted_Connection=True;";

        public SpendingPage()
        {
            InitializeComponent();
            LoadSpendingData();
        }

        private void LoadSpendingData()
        {
            UpdatePieChart(DateTime.Now.AddMonths(-1), DateTime.Now); 
        }

        private void UpdatePieChart(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT Category, SUM(Amount) AS TotalAmount FROM [Spending] WHERE TransactionDate >= @StartDate AND TransactionDate <= @EndDate GROUP BY Category";
            var values = new ChartValues<double>();
            var labels = new List<string>();
            double totalSpending = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labels.Add(reader["Category"].ToString());
                            double amount = reader.GetDouble(reader.GetOrdinal("TotalAmount"));
                            values.Add(amount);
                            totalSpending += amount; 
                        }
                    }
                }
            }

            pieChart.Series.Clear();
            for (int i = 0; i < labels.Count; i++)
            {
                pieChart.Series.Add(new PieSeries
                {
                    Title = labels[i],
                    Values = new ChartValues<double> { values[i] },
                    DataLabels = true 
                });
            }

            TotalSpendingTextBlock.Text = $"₽ {totalSpending:0.00}"; 
        }

        private void AddSpendingButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSpendingDialog();
            if (dialog.ShowDialog() == true) 
            {
                double amount;
                
                if (double.TryParse(dialog.SpendingAmountTextBox.Text, out amount))
                {
                    DateTime transactionDate = DateTime.Now;  

                     
                    AddSpendingToDatabase(amount, transactionDate);
                    LoadSpendingData();  
                }
                else
                {
                    MessageBox.Show("Введите корректную сумму.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddCategoryDialog();
            if (dialog.ShowDialog() == true) 
            {
                string categoryName = dialog.CategoryNameTextBox.Text;

                
                AddCategoryToDatabase(categoryName);
                LoadSpendingData(); 
            }
        }

        private void AddSpendingToDatabase(double amount, DateTime transactionDate)
        {
            string query = "INSERT INTO [Spending] (Amount, TransactionDate, Category) VALUES (@Amount, @TransactionDate, @Category)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@TransactionDate", transactionDate);
                        command.Parameters.AddWithValue("@Category", "DefaultCategory");  
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка при добавлении расхода: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCategoryToDatabase(string categoryName)
        {
            string query = "INSERT INTO [Categories] (CategoryName) VALUES (@CategoryName)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", categoryName);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка при добавлении категории: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PeriodButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
