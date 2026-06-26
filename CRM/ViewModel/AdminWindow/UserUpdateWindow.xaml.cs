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
        PasswordBox.Text = user.Password;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string login = LoginBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text?.Trim() ?? "";

        if (!Validation.IsEnglish(login) || !Validation.IsEnglish(password))
        {
            MessageBox.Show(
                "Login and password must contain only English letters and numbers",
                "Invalid characters",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }
        
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show(
                "The password or login is empty",
                "Empty",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (login.Length <= 3 || password.Length <= 3)
        {
            MessageBox.Show(
                "The password or login is too short",
                "Short",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(u => u.Id == _userId);

        if (user == null)
            return;

        bool userExists = db.Users.Any(u => u.Name == login && u.Id != _userId);

        if (userExists)
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

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}