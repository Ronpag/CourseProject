using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class RegisterClientWindow : Window
{
    public RegisterClientWindow()
    {
        InitializeComponent();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string name = ClientNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Client name is empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        if (db.Clients.Any(c => c.NameClient == name))
        {
            MessageBox.Show("This client already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

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

        MessageBox.Show("Client created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        DialogResult = true;
        Close();
    }
}
