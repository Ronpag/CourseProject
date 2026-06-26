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

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}