using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginBtn(object sender, RoutedEventArgs e)
    {
        string login = Login.Text?.Trim() ?? "";
        string password = Password.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Empty login or password", "Error");
            return;
        }

        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(u => u.Name == login);

        if (user != null)
        {
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                MessageBox.Show("Invalid login or password", "Error");
                return;
            }

            if (!user.IsActive)
            {
                MessageBox.Show(
                    "Your account is disabled. Please contact administrator.",
                    "Access denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            Window window = user.IsAdmin
                ? new AdminWindow()
                : new UserWindow(user.Id);

            window.Show();
            Close();
            return;
        }

        var client = db.Clients.FirstOrDefault(c => c.Login == login);

        if (client == null || !BCrypt.Net.BCrypt.Verify(password, client.Password))
        {
            MessageBox.Show("Invalid login or password", "Error");
            return;
        }

        var clientWindow = new ClientWindow(client.Id);
        clientWindow.Show();
        Close();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        var window = new ClientRegistrationWindow();

        if (window.ShowDialog() == true)
        {
            MessageBox.Show(
                "Registration successful! You can now log in.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}