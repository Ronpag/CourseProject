using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class ClientPage : Page
{
    public ClientPage()
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

        var client = new Client
        {
            NameClient = nameClient,
            CountOrders = 0
        };

        db.Client.Add(client);
        db.SaveChanges();

        LoadClients();

        MessageBox.Show(
            "Client created",
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

        var client = db.Client.FirstOrDefault(c => c.Id == selectedClient.Id);

        if (client == null)
            return;

        db.Client.Remove(client);
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

        ClientsList.ItemsSource = db.Client.ToList();
    }
}