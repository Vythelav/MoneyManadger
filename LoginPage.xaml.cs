using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyManadger
{
    public partial class LoginPage : Page
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=MoneyManager;Trusted_Connection=True;";
        private User currentUser;

        public LoginPage() 
        {
            InitializeComponent();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                password = builder.ToString();

                currentUser = ValidateUser(username, password); 

                if (currentUser != null)
                {
                    ManagerWindow managerWindow = new ManagerWindow(currentUser);
                    managerWindow.Show();
                    
                   // Window.GetWindow(this)?.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private User ValidateUser(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Login FROM Users WHERE Login = @login AND Password = @password"; 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]), 
                                    Name = reader["Login"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }

   
}
