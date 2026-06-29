using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class ClientRegistrationWindow : Window
{
    public ClientRegistrationWindow()
    {
        InitializeComponent();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string name = NameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Enter client name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        bool loginExists = db.Clients.Any(c => c.Login == login);

        if (loginExists)
        {
            MessageBox.Show("Login already taken", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        db.Clients.Add(new Client
        {
            NameClient = name,
            Login = login,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            CountOrders = 0
        });

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}
