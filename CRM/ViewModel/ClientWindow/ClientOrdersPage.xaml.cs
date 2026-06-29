using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.ClientWindow;

public partial class ClientOrdersPage : Page
{
    private readonly int _clientId;

    public ClientOrdersPage(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;
        LoadOrders();
    }

    private void LoadOrders()
    {
        using var db = new AppDbContext();

        OrdersList.ItemsSource = db.Tasks
            .Where(t => t.ClientId == _clientId)
            .OrderByDescending(t => t.Id)
            .ToList();
    }

    private void DeleteOrderBtn(object sender, RoutedEventArgs e)
    {
        if (OrdersList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select an order");
            return;
        }

        if (selectedTask.Status != CRM.Data.Task.TaskStatus.Pending)
        {
            MessageBox.Show("Only pending orders can be deleted.");
            return;
        }

        var result = MessageBox.Show(
            "Delete this order permanently?",
            "Confirm deletion",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == selectedTask.Id);

        if (task == null)
            return;

        var client = db.Clients.FirstOrDefault(c => c.Id == _clientId);

        if (client != null)
            client.CountOrders--;

        db.Tasks.Remove(task);

        db.SaveChanges();

        MessageBox.Show("Order deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadOrders();
    }
}
