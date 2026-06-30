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
        if (OrdersList == null) return;

        var statusFilters = new List<CRM.Data.Task.TaskStatus>();
        if (ChkPending.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Pending);
        if (ChkAvailable.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Available);
        if (ChkAssigned.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Assigned);
        if (ChkInProgress.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.InProgress);
        if (ChkCompleted.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Completed);

        var tasks = TaskService.GetFiltered(
            clientId: _clientId, statusFilters: statusFilters);

        OrdersList.ItemsSource = tasks.OrderByDescending(t => t.Id).ToList();
    }

    private void FilterChanged(object sender, RoutedEventArgs e)
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

        if (TaskService.Delete(selectedTask.Id))
        {
            MessageBox.Show("Order deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadOrders();
        }
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
