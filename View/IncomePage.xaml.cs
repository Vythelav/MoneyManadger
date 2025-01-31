using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using LiveCharts;
using LiveCharts.Wpf;

namespace MoneyManager
{
    public partial class IncomePage : Page
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=MoneyManager;Trusted_Connection=True;";

        public IncomePage()
        {
            InitializeComponent();
            UpdateTotalIncome();
            UpdatePieChart();
        }

        private void UpdateTotalIncome()
        {
            string query = "SELECT SUM(Amount) FROM [Transaction]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var total = command.ExecuteScalar();
                    TotalIncomeTextBlock.Text = total != DBNull.Value ? $"₽ {total}" : "₽ 0";
                }
            }
        }

        private void UpdatePieChart()
        {
            string query = "SELECT Category, SUM(Amount) AS TotalAmount FROM [Transaction] GROUP BY Category";
            var values = new ChartValues<double>();
            var labels = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labels.Add(reader["Category"].ToString());
                            values.Add(Convert.ToDouble(reader["TotalAmount"]));
                        }
                    }
                }
            }

            pieChart.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Доходы",
                    Values = values
                }
            };

            pieChart.LegendLocation = LegendLocation.Right;
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            var addIncomeWindow = new AddIncomeWindow();
            if (addIncomeWindow.ShowDialog() == true)
            {
                string category = addIncomeWindow.Category;
                decimal amount = addIncomeWindow.Amount;

                int categoryId = GetCategoryId(category);

                if (categoryId == 0)
                {
                    MessageBox.Show("Категория не найдена. Пожалуйста, проверьте введенное название категории.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO [Transaction] (CategoryId, Amount) VALUES (@CategoryId, @Amount)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.ExecuteNonQuery();
                    }
                }

                UpdateTotalIncome();
                UpdatePieChart();
            }
        }

        private int GetCategoryId(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Id FROM Categories WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryName);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }


        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow();
            if (addCategoryWindow.ShowDialog() == true)
            {
                string newCategory = addCategoryWindow.CategoryName;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO Categories (Name) VALUES (@Name)"; using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", newCategory);
                        command.ExecuteNonQuery();
                    }
                }

                UpdatePieChart();
            }
        }

        private void PeriodButton_Click(object sender, RoutedEventArgs e)
        {

            var periodWindow = new PeriodSelectionWindow();
            if (periodWindow.ShowDialog() == true)
            {
                DateTime startDate = periodWindow.StartDate;
                DateTime endDate = periodWindow.EndDate;

                UpdateTotalIncome(startDate, endDate);
                UpdatePieChart(startDate, endDate);
            }
        }

        private void UpdateTotalIncome(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT SUM(Amount) FROM [Transaction] WHERE TransactionDate >= @StartDate AND TransactionDate <= @EndDate";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    var total = command.ExecuteScalar();
                    TotalIncomeTextBlock.Text = total != DBNull.Value ? $"₽ {total}" : "₽ 0";
                }
            }
        }

        private void UpdatePieChart(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT Category, SUM(Amount) AS TotalAmount FROM [Transaction] WHERE TransactionDate >= @StartDate AND TransactionDate <= @EndDate GROUP BY Category";
            var values = new ChartValues<double>();
            var labels = new List<string>();

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
                            values.Add(Convert.ToDouble(reader["TotalAmount"]));
                        }
                    }
                }
            }

            pieChart.Series.Clear();
            foreach (var label in labels)
            {
                pieChart.Series.Add(new PieSeries
                {
                    Title = label,
                    Values = values
                });
            }

            pieChart.LegendLocation = LegendLocation.Right;
        }
    }
}
