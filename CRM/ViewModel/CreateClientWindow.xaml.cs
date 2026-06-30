using System.Windows;

namespace CRM.ViewModel.UserWindow;

public partial class CreateClientWindow : Window
{
    public CreateClientWindow()
    {
        InitializeComponent();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string name = ClientNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (ClientService.Create(name, login, password))
        {
            DialogResult = true;
            Close();
        }
    }
}
