using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class UserUpdateWindow : Window
{
    private readonly User _user;

    public UserUpdateWindow(User user)
    {
        InitializeComponent();

        _user = user;

        UserNameBox.Text = user.UserName;
        LoginBox.Text = user.Name;
        PasswordBox.Text = "";

        IsActiveBox.IsChecked = user.IsActive;

        if (user.IsAdmin)
            IsActiveBox.IsEnabled = false;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string UserName = UserNameBox.Text.Trim();
        string login = LoginBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text?.Trim() ?? "";

        if (!ValidationService.ValidateEnglishText(UserName, "User name"))
            return;

        bool? isActive = _user.IsAdmin ? null : (bool?)IsActiveBox.IsChecked;

        if (UserService.Update(_user.Id, login, UserName,
                string.IsNullOrWhiteSpace(password) ? null : password, isActive))
        {
            DialogResult = true;
            Close();
        }
    }
}
