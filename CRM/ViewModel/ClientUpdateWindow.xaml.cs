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

        if (!ValidationService.ValidateEnglishText(newName, "Client name"))
            return;

        if (ClientService.Update(_client.Id, newName, newLogin,
                string.IsNullOrWhiteSpace(newPassword) ? null : newPassword, countOrders))
        {
            DialogResult = true;
            Close();
        }
    }
}
