using System.Windows;
using CRM.ViewModel.ClientWindow;

namespace CRM.View;

public partial class ClientWindow : Window
{
    private readonly int _clientId;

    public ClientWindow(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;
    }

    private void BackBtn(object sender, RoutedEventArgs e)
    {
        LoginWindow login = new LoginWindow();
        login.Show();

        Close();
    }

    private void MyOrdersBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ClientOrdersPage(_clientId));
    }

    private void CreateOrderBtn(object sender, RoutedEventArgs e)
    {
        var window = new CreateOrderWindow(_clientId);

        if (window.ShowDialog() == true)
        {
            MainFrame.Navigate(new ClientOrdersPage(_clientId));
        }
    }
}
