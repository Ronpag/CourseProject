using System.Windows;

namespace CRM.View;

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
        string email = EmailBox.Text.Trim();
        string phone = PhoneBox.Text.Trim();
        string address = AddressBox.Text.Trim();

        if (!ValidationService.ValidateEnglishText(name, "Client name"))
            return;

        if (ClientService.Create(name, login, password,
                email: string.IsNullOrWhiteSpace(email) ? null : email,
                phone: string.IsNullOrWhiteSpace(phone) ? null : phone,
                address: string.IsNullOrWhiteSpace(address) ? null : address))
        {
            DialogResult = true;
            Close();
        }
    }
}
