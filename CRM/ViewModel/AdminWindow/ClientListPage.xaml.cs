using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class ClientListPage : Page
{
    public ClientListPage()
    {
        InitializeComponent();
        LoadClients();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string nameClient = NameClient.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(nameClient))
        {
            MessageBox.Show(
                "Client name is empty",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        using var db = new AppDbContext();

        bool clientExists = db.Clients.Any(c => c.NameClient == nameClient);

        if (clientExists)
        {
            MessageBox.Show(
                "This client already exists",
                "Client",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var client = new Client
        {
            NameClient = nameClient,
            CountOrders = 0
        };

        db.Clients.Add(client);
        db.SaveChanges();

        LoadClients();

        MessageBox.Show(
            "Client was created",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void DeleteBtn(object sender, RoutedEventArgs e)
    {
        if (ClientsList.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show("Select client");
            return;
        }

        using var db = new AppDbContext();

        var client = db.Clients.FirstOrDefault(c => c.Id == selectedClient.Id);

        if (client == null)
            return;

        db.Clients.Remove(client);
        db.SaveChanges();

        LoadClients();

        MessageBox.Show("Client deleted");
    }

    private void UpdateBtn(object sender, RoutedEventArgs e)
    {
        if (ClientsList.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show(
                "Select client",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var window = new ClientUpdateWindow(selectedClient);

        bool? result = window.ShowDialog();

        if (result == true)
        {
            LoadClients();

            MessageBox.Show(
                "Client updated",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void LoadClients()
    {
        using var db = new AppDbContext();

        ClientsList.ItemsSource = db.Clients.ToList();
    }
}