using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class UserUpdateWindow : Window
{
    private readonly int _userId;
    private readonly bool _isAdmin;

    public UserUpdateWindow(User user)
    {
        InitializeComponent();

        _userId = user.Id;
        _isAdmin = user.IsAdmin;

        LoginBox.Text = user.Name;
        PasswordBox.Text = user.Password;

        IsActiveBox.IsChecked = user.IsActive;

        // ❗ Админа нельзя редактировать
        if (user.IsAdmin)
        {
            IsActiveBox.IsEnabled = false;
        }
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string login = LoginBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text?.Trim() ?? "";

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(u => u.Id == _userId);

        if (user == null)
            return;

        if (db.Users.Any(u => u.Name == login && u.Id != _userId))
        {
            MessageBox.Show(
                "This login already exists",
                "Login",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        user.Name = login;
        user.Password = password;

        if (!user.IsAdmin)
        {
            user.IsActive = IsActiveBox.IsChecked == true;
        }

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}