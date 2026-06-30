using System.Windows;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class CreateClientWindow : Window
{
    public CreateClientWindow()
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
            MessageBox.Show("Enter client name");
            return;
        }

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        if (db.Clients.Any(c => c.NameClient == name))
        {
            MessageBox.Show("Client already exists");
            return;
        }

        bool loginExists = db.Clients.Any(c => c.Login == login);

        if (loginExists)
        {
            MessageBox.Show("Login already taken");
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
