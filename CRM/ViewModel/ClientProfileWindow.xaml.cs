using System.Linq;
using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class ClientProfileWindow : Window
{
    private readonly int _clientId;

    public ClientProfileWindow(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;

        using var db = new AppDbContext();
        var client = db.Clients.FirstOrDefault(c => c.Id == clientId);

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

        if (!string.IsNullOrWhiteSpace(email) && !Validation.ValidateEmail(email))
            return;

        if (!string.IsNullOrWhiteSpace(phone) && !Validation.ValidatePhone(phone))
            return;

        using var db = new AppDbContext();
        var client = db.Clients.FirstOrDefault(c => c.Id == _clientId);

        if (client == null)
        {
            MessageBox.Show("Client not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        bool hasChanges = false;

        if (email != (client.Email ?? "")) hasChanges = true;
        if (phone != (client.Phone ?? "")) hasChanges = true;
        if (address != (client.Address ?? "")) hasChanges = true;

        if (!hasChanges)
        {
            MessageBox.Show("No changes to submit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var request = new ProfileChangeRequest
        {
            ClientId = _clientId,
            NewEmail = email != (client.Email ?? "") ? email : null,
            NewPhone = phone != (client.Phone ?? "") ? phone : null,
            NewAddress = address != (client.Address ?? "") ? address : null,
            IsProcessed = false
        };

        db.ProfileChangeRequests.Add(request);
        db.SaveChanges();

        MessageBox.Show(
            "Profile change request submitted for admin approval.",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        DialogResult = true;
        Close();
    }
}
