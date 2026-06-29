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

        LoginBox.Text = user.Name;
        PasswordBox.Text = ""; // пароль НЕ показываем

        IsActiveBox.IsChecked = user.IsActive;

        if (user.IsAdmin)
            IsActiveBox.IsEnabled = false;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string login = LoginBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text?.Trim() ?? "";

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