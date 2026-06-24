using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class ClientUpdateWindow : Window
{
    private readonly int _clientId;

    public ClientUpdateWindow(Client client)
    {
        InitializeComponent();

        _clientId = client.Id;

        NameClientBox.Text = client.NameClient;
        CountOrdersBox.Text = client.CountOrders.ToString();
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(CountOrdersBox.Text, out int countOrders))
        {
            MessageBox.Show("Invalid count orders");
            return;
        }

        using var db = new AppDbContext();

        var client = db.Client.FirstOrDefault(c => c.Id == _clientId);

        if (client == null)
            return;

        client.NameClient = NameClientBox.Text.Trim();
        client.CountOrders = countOrders;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}