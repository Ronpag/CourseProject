using System.Windows;

namespace CRM.View;

public partial class ClientRegistrationWindow : Window
{
    public ClientRegistrationWindow()
    {
        InitializeComponent();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string name = NameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (ClientService.Create(name, login, password))
        {
            DialogResult = true;
            Close();
        }
    }
}
