using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

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
        var window = new RegisterClientWindow();

        if (window.ShowDialog() == true)
        {
            LoadClients();

            MessageBox.Show(
                "Client was created",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
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

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if (ClientsList.SelectedItem is not Client client) return;
        new DetailsWindow(client).ShowDialog();
    }

    private void ClientsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ClientsList.SelectedItem is not Client client) return;
        new DetailsWindow(client).ShowDialog();
    }
}