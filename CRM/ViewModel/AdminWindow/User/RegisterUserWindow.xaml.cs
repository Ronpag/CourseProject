using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class RegisterUserWindow : Window
{
    public RegisterUserWindow()
    {
        InitializeComponent();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string workerName = WorkerNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (string.IsNullOrWhiteSpace(workerName))
        {
            MessageBox.Show("Enter worker name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        bool userExists = db.Users.Any(u =>
            u.Name.ToLower() == login.ToLower());

        if (userExists)
        {
            MessageBox.Show(
                "User already exists",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        db.Users.Add(new User
        {
            Name = login,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            WorkerName = workerName,
            IsAdmin = false,
            IsActive = ActiveCheckBox.IsChecked == true
        });

        db.SaveChanges();

        MessageBox.Show(
            "User created",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        DialogResult = true;
        Close();
    }
}
