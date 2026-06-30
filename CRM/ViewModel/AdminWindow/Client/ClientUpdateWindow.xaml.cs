using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class ClientUpdateWindow : Window
{
    private readonly int _clientId;

    public ClientUpdateWindow(Client client)
    {
        InitializeComponent();

        _clientId = client.Id;

        NameClientBox.Text = client.NameClient;
        LoginBox.Text = client.Login;
        PasswordBox.Text = "";
        CountOrdersBox.Text = client.CountOrders.ToString();
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(CountOrdersBox.Text, out int countOrders))
        {
            MessageBox.Show("Invalid count orders", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string newName = NameClientBox.Text.Trim();
        string newLogin = LoginBox.Text.Trim();
        string newPassword = PasswordBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(newName))
        {
            MessageBox.Show("Client name is empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(newLogin))
        {
            MessageBox.Show("Login is empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var db = new AppDbContext();

        bool clientExists = db.Clients.Any(c =>
            c.Id != _clientId &&
            c.NameClient.ToLower() == newName.ToLower());

        if (clientExists)
        {
            MessageBox.Show("Client with this name already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        bool loginExists = db.Clients.Any(c =>
            c.Id != _clientId &&
            c.Login == newLogin);

        if (loginExists)
        {
            MessageBox.Show("Login already taken", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var client = db.Clients.FirstOrDefault(c => c.Id == _clientId);

        if (client == null)
        {
            MessageBox.Show("Client not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        client.NameClient = newName;
        client.Login = newLogin;

        if (!string.IsNullOrWhiteSpace(newPassword))
            client.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

        client.CountOrders = countOrders;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}
