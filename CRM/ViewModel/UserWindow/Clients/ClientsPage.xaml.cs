using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.ViewModel.UserWindow;

public partial class ClientsPage : Page
{
    public ClientsPage()
    {
        InitializeComponent();
        LoadClients();
    }

    private void LoadClients()
    {
        using var db = new AppDbContext();

        var query = db.Clients.AsQueryable();

        string filter = SearchBox?.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(c => c.NameClient.Contains(filter));

        ClientsList.ItemsSource = query.ToList();
    }

    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        LoadClients();
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
