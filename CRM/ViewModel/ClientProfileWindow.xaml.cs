using System.Windows;

namespace CRM.View;

public partial class ClientProfileWindow : Window
{
    private readonly int _clientId;

    public ClientProfileWindow(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;

        var client = ClientService.GetById(clientId);

        if (client == null)
        {
            MessageBox.Show("Client not found");
            Close();
            return;
        }

        NameText.Text = client.NameClient;
        LoginText.Text = client.Login;
        EmailBox.Text = client.Email ?? "";
        PhoneBox.Text = client.Phone ?? "";
        AddressBox.Text = client.Address ?? "";
    }

    private void SubmitBtn(object sender, RoutedEventArgs e)
    {
        string email = EmailBox.Text.Trim();
        string phone = PhoneBox.Text.Trim();
        string address = AddressBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(address) && !ValidationService.IsEnglishText(address))
        {
            MessageBox.Show("Address must contain only English characters", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (RequestService.CreateProfileChangeRequest(
                userId: null, clientId: _clientId,
                email, phone, address, isForUser: false))
        {
            DialogResult = true;
            Close();
        }
    }
}
