using System.Windows;

namespace CRM.View;

public partial class RegisterClientWindow : Window
{
    public RegisterClientWindow()
    {
        InitializeComponent();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string name = ClientNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (!ValidationService.ValidateEnglishText(name, "Client name"))
            return;

        if (ClientService.Create(name, login, password))
        {
            DialogResult = true;
            Close();
        }
    }
}
