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
        if (ClientsList == null) return;
        ClientsList.ItemsSource = ClientService.GetAll();
    }

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Client client) return;
        new DetailsWindow(client).ShowDialog();
    }

    private void ClientsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ClientsList.SelectedItem is not Client client) return;
        new DetailsWindow(client).ShowDialog();
    }
}