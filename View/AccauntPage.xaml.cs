using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoneyManadger
{
    public partial class AccauntPage : Page
    {
        private string connectionString = "Server=510EC15;Database=MoneyManager;Trusted_Connection=True;";
        private int currentUserId;

        public AccauntPage(User user)
        {
            InitializeComponent();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Пользователь не может быть null");
            }

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
                    accountInfoTextBlock.Text = "Пользователь не найден"; nameTextBlock.Text = "";
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

        private async void ChangeAccountDataButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = PromptForNewName(); // Теперь открывается диалог для ввода нового имени
            if (!string.IsNullOrEmpty(newName))
            {
                await UpdateAccountDataAsync(currentUserId, newName); // Обновление данных
                LoadUserData(currentUserId); // Перезагрузка данных
            }
        }

        private async void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить аккаунт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await DeleteAccountAsync(currentUserId); // Удаление аккаунта
                MessageBox.Show("Аккаунт успешно удален.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Перенаправление на страницу входа или закрытие приложения
                NavigationService.Navigate(new LoginPage()); // Предполагается, что у вас есть такая страница
            }
        }

        private string PromptForNewName()
        {
            NameInputDialog nameInputDialog = new NameInputDialog();
            if (nameInputDialog.ShowDialog() == true) // Проверяем, что диалог был закрыт с результатом "OK"
            {
                return nameInputDialog.NewName; // Возвращаем новое имя
            }
            return null; 
        }

        private async Task UpdateAccountDataAsync(int userId, string newName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "UPDATE Users SET Name = @newName WHERE Id = @userId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newName", newName);
                        command.Parameters.AddWithValue("@userId", userId);

                        await command.ExecuteNonQueryAsync(); // Асинхронное выполнение запроса
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteAccountAsync(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Users WHERE Id = @userId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        await command.ExecuteNonQueryAsync(); // Асинхронное выполнение запроса
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении аккаунта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

   
}
