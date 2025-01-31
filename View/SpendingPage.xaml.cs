using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using MoneyManadger;

namespace MoneyManager
{
    public partial class SpendingPage : Page
    {
        private readonly string connectionString = "Server=510EC15;Database=MoneyManager;Trusted_Connection=True;";
        private readonly Dictionary<string, int> categories = new Dictionary<string, int>();

        public SpendingPage()
        {
            InitializeComponent();
            LoadCategories();
            LoadSpendingData();
        }

        private void LoadCategories()
        {
            categories.Clear();
            string query = "SELECT Id, Name FROM Categories";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                categories[name] = id;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSpendingData()
        {
            UpdatePieChart(DateTime.Now.AddMonths(-1), DateTime.Now);
        }

        private void UpdatePieChart(DateTime startDate, DateTime endDate)
        {
            string query = @"
                SELECT c.Name, SUM(s.Amount) AS TotalAmount 
                FROM Spending s 
                JOIN Categories c ON s.CategoryId = c.Id 
                WHERE s.TransactionDate BETWEEN @StartDate AND @EndDate 
                GROUP BY c.Name";

            var values = new ChartValues<double>();
            var labels = new List<string>();
            double totalSpending = 0;

            try
            {
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
                                labels.Add(reader["Name"].ToString());
                                double amount = Convert.ToDouble(reader["TotalAmount"]);
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
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddSpendingButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSpendingDialog(categories.Keys);
            if (dialog.ShowDialog() == true)
            {
                int amount = dialog.Amount;
                string categoryName = dialog.Category;
                DateTime transactionDate = DateTime.Now;

                if (categories.TryGetValue(categoryName, out int categoryId))
                {
                    AddSpendingToDatabase(amount, categoryId, transactionDate);
                    LoadSpendingData();
                }
                else
                {
                    MessageBox.Show("Ошибка: выбранной категории не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddSpendingToDatabase(int amount, int categoryId, DateTime transactionDate)
        {
            string query = "INSERT INTO Spending (Amount, CategoryId, TransactionDate) VALUES (@Amount, @CategoryId, @TransactionDate)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@TransactionDate", transactionDate);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка при добавлении расхода: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PeriodButton_Click(object sender, RoutedEventArgs e)
        {
            // Реализация метода выбора периода
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryDialog dialog = new AddCategoryDialog();
            if (dialog.ShowDialog() == true)
            {
                string newCategoryName = dialog.CategoryName;
                if (AddCategoryToDatabase(newCategoryName))
                {
                    MessageBox.Show($"Категория '{newCategoryName}' успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCategories(); // Обновляем список категорий
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении категории. Возможно, такая категория уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool AddCategoryToDatabase(string categoryName)
        {
            string query = "INSERT INTO Categories (Name) VALUES (@Name)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", categoryName);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (SqlException ex) when (ex.Number == 2627) // Violation of unique constraint
            {
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении категории: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}

    
