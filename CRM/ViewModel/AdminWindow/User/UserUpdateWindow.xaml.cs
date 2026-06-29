using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class UserUpdateWindow : Window
{
    private readonly int _userId;

    public UserUpdateWindow(User user)
    {
        InitializeComponent();

        _userId = user.Id;

        WorkerNameBox.Text = user.WorkerName;
        LoginBox.Text = user.Name;
        PasswordBox.Text = "";

        IsActiveBox.IsChecked = user.IsActive;

        if (user.IsAdmin)
            IsActiveBox.IsEnabled = false;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string workerName = WorkerNameBox.Text.Trim();
        string login = LoginBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(workerName))
        {
            MessageBox.Show("Enter worker name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!Validation.ValidateLoginPassword(login, "1234"))
            return;

        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(u => u.Id == _userId);

        if (user == null) return;

        if (db.Users.Any(u => u.Name == login && u.Id != _userId))
        {
            MessageBox.Show("Login already exists");
            return;
        }

        user.WorkerName = workerName;
        user.Name = login;

        if (!string.IsNullOrWhiteSpace(password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        if (!user.IsAdmin)
        {
            user.IsActive = IsActiveBox.IsChecked == true;
        }

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}
