using System.Windows;

namespace CRM.View;

public partial class RegisterUserWindow : Window
{
    public RegisterUserWindow()
    {
        InitializeComponent();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string UserName = UserNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (!ValidationService.ValidateEnglishText(UserName, "User name"))
            return;

        if (UserService.Create(login, UserName, password, isAdmin: false, isActive: ActiveCheckBox.IsChecked == true))
        {
            DialogResult = true;
            Close();
        }
    }
}
