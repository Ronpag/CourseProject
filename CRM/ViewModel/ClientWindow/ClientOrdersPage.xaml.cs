using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

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

        var query = db.Tasks
            .Where(t => t.ClientId == _clientId);

        string filter = SearchBox?.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(t => t.TaskName.Contains(filter));

        OrdersList.ItemsSource = query
            .OrderByDescending(t => t.Id)
            .ToList();
    }

    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        LoadOrders();
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

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if (OrdersList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }

    private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (OrdersList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }
}
