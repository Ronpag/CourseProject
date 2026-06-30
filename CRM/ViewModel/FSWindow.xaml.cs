using System.Windows;
using System.Windows.Controls;

namespace CRM.View;

public partial class FSWindow : Window
{
    public FSWindow()
    {
        InitializeComponent();
    }

    private void FirstLoginBtn(object sender, RoutedEventArgs e)
    {
        string UserName = FUserName.Text?.Trim() ?? "";
        string login = FLogin.Text?.Trim() ?? "";
        string password = FPassword.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(UserName))
        {
            MessageBox.Show("Enter User name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ValidationService.ValidateEnglishText(UserName, "User name"))
            return;

        if (!ValidationService.ValidateLoginPassword(login, password))
            return;

        if (UserService.Create(login, UserName, password, isAdmin: true, isActive: true))
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
