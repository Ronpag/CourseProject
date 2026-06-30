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
        string workerName = WorkerNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (UserService.Create(login, workerName, password, isAdmin: false, isActive: ActiveCheckBox.IsChecked == true))
        {
            DialogResult = true;
            Close();
        }
    }
}
