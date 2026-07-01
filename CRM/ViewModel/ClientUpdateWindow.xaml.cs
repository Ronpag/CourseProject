using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class ClientUpdateWindow : Window
{
    private readonly Client _client;

    public ClientUpdateWindow(Client client)
    {
        InitializeComponent();

        _client = client;

        NameClientBox.Text = client.NameClient;
        LoginBox.Text = client.Login;
        PasswordBox.Text = "";
        CountOrdersBox.Text = client.CountOrders.ToString();
        EmailBox.Text = client.Email ?? "";
        PhoneBox.Text = client.Phone ?? "";
        AddressBox.Text = client.Address ?? "";
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(CountOrdersBox.Text, out int countOrders))
        {
            MessageBox.Show("Invalid count orders", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string newName = NameClientBox.Text.Trim();
        string newLogin = LoginBox.Text.Trim();
        string newPassword = PasswordBox.Text.Trim();
        string email = EmailBox.Text.Trim();
        string phone = PhoneBox.Text.Trim();
        string address = AddressBox.Text.Trim();

        if (!ValidationService.ValidateEnglishText(newName, "Client name"))
            return;

        if (!ValidationService.ValidateEmail(email))
            return;

        if (!ValidationService.ValidatePhone(phone))
            return;

        if (ClientService.Update(_client.Id, newName, newLogin,
                string.IsNullOrWhiteSpace(newPassword) ? null : newPassword, countOrders,
                email: string.IsNullOrWhiteSpace(email) ? null : email,
                phone: string.IsNullOrWhiteSpace(phone) ? null : phone,
                address: string.IsNullOrWhiteSpace(address) ? null : address))
        {
            DialogResult = true;
            Close();
        }
    }
}
