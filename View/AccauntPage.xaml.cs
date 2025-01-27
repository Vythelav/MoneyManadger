using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MoneyManadger
{
    public partial class AccauntPage : Page
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=MoneyManager;Trusted_Connection=True;";

        private int currentUserId;

        public AccauntPage(User user)
        {
            InitializeComponent();
            currentUserId = user.Id;
            LoadUserData(currentUserId);
        }

        private async void LoadUserData(int userId)
        {
            try
            {
                User user = await GetUserAsync(userId);
                if (user != null)
                {
                    accountInfoTextBlock.Text = "Информация об аккаунте";
                    nameTextBlock.Text = user.Name;
                    idTextBlock.Text = $"ID: {user.Id}";
                }
                else
                {
                    accountInfoTextBlock.Text = "Пользователь не найден";
                    nameTextBlock.Text = "";
                    idTextBlock.Text = "";
                }
            }
            catch (Exception ex)
            {
                accountInfoTextBlock.Text = "Ошибка загрузки данных";
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private async Task<User> GetUserAsync(int userId)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Name, Id FROM Users WHERE Id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Name = reader["Name"].ToString(),
                                Id = Convert.ToInt32(reader["Id"])
                            };
                        }
                    }
                }
            }

            return user;
        }
    }

  
}
