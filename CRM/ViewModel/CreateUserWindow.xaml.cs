using System.Windows;

namespace CRM.View;

public partial class CreateUserWindow : Window
{
    public CreateUserWindow()
    {
        InitializeComponent();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string UserName = UserNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();
        string email = EmailBox.Text.Trim();
        string phone = PhoneBox.Text.Trim();
        string position = PositionBox.Text.Trim();

        if (!ValidationService.ValidateEnglishText(UserName, "User name"))
            return;

        if (UserService.Create(login, UserName, password, isAdmin: false, isActive: ActiveCheckBox.IsChecked == true,
                email: string.IsNullOrWhiteSpace(email) ? null : email,
                phone: string.IsNullOrWhiteSpace(phone) ? null : phone,
                position: string.IsNullOrWhiteSpace(position) ? null : position))
        {
            DialogResult = true;
            Close();
        }
    }
}
