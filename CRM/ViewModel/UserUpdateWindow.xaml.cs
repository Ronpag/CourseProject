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
        EmailBox.Text = user.Email ?? "";
        PhoneBox.Text = user.Phone ?? "";
        PositionBox.Text = user.Position ?? "";

        IsActiveBox.IsChecked = user.IsActive;

        if (user.IsAdmin)
            IsActiveBox.IsEnabled = false;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string UserName = UserNameBox.Text.Trim();
        string login = LoginBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text?.Trim() ?? "";
        string email = EmailBox.Text.Trim();
        string phone = PhoneBox.Text.Trim();
        string position = PositionBox.Text.Trim();

        if (!ValidationService.ValidateEnglishText(UserName, "User name"))
            return;

        if (!ValidationService.ValidateEmail(email))
            return;

        if (!ValidationService.ValidatePhone(phone))
            return;

        bool? isActive = _user.IsAdmin ? null : (bool?)IsActiveBox.IsChecked;

        if (UserService.Update(_user.Id, login, UserName,
                string.IsNullOrWhiteSpace(password) ? null : password, isActive,
                email: string.IsNullOrWhiteSpace(email) ? null : email,
                phone: string.IsNullOrWhiteSpace(phone) ? null : phone,
                position: string.IsNullOrWhiteSpace(position) ? null : position))
        {
            DialogResult = true;
            Close();
        }
    }
}
