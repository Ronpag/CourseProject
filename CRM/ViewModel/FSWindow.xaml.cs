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
        string workerName = FWorkerName.Text?.Trim() ?? "";
        string login = FLogin.Text?.Trim() ?? "";
        string password = FPassword.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(workerName))
        {
            MessageBox.Show("Enter worker name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ValidationService.ValidateLoginPassword(login, password))
            return;

        if (UserService.Create(login, workerName, password, isAdmin: true, isActive: true))
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
