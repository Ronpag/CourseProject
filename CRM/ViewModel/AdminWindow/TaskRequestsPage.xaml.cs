using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.AdminWindow;

public partial class TaskRequestsPage : Page
{
    public TaskRequestsPage()
    {
        InitializeComponent();
        LoadRequests();
        LoadPendingOrders();
    }

    private void LoadRequests()
    {
        using var db = new AppDbContext();

        RequestsList.ItemsSource = db.TaskStatusRequests
            .Where(r => !r.IsProcessed)
            .ToList();
    }

    private void LoadPendingOrders()
    {
        using var db = new AppDbContext();

        PendingOrdersList.ItemsSource = db.Tasks
            .Where(t => t.Status == CRM.Data.Task.TaskStatus.Pending)
            .ToList();
    }

    private void ApproveBtn(object sender, RoutedEventArgs e)
    {
        if (RequestsList.SelectedItem is not TaskStatusRequest request)
        {
            MessageBox.Show("Select request");
            return;
        }

        using var db = new AppDbContext();

        var dbRequest = db.TaskStatusRequests
            .FirstOrDefault(r => r.Id == request.Id);

        if (dbRequest == null)
            return;

        var task = db.Tasks.FirstOrDefault(t => t.Id == dbRequest.TaskId);

        if (task == null)
        {
            MessageBox.Show("Task not found");
            return;
        }

        task.Status = dbRequest.RequestedStatus;

        if (dbRequest.RequestedStatus == CRM.Data.Task.TaskStatus.Completed)
            task.CompletionDate = DateTime.Now;

        dbRequest.IsProcessed = true;
        dbRequest.IsApproved = true;

        db.SaveChanges();

        MessageBox.Show("Status request approved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadRequests();
    }

    private void RejectBtn(object sender, RoutedEventArgs e)
    {
        if (RequestsList.SelectedItem is not TaskStatusRequest request)
        {
            MessageBox.Show("Select request");
            return;
        }

        using var db = new AppDbContext();

        var dbRequest = db.TaskStatusRequests
            .FirstOrDefault(r => r.Id == request.Id);

        if (dbRequest == null)
            return;

        dbRequest.IsProcessed = true;
        dbRequest.IsApproved = false;

        db.SaveChanges();

        MessageBox.Show("Status request rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadRequests();
    }

    private void ApproveOrderBtn(object sender, RoutedEventArgs e)
    {
        if (PendingOrdersList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select order");
            return;
        }

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == selectedTask.Id);

        if (task == null)
            return;

        task.Status = CRM.Data.Task.TaskStatus.Available;

        db.SaveChanges();

        MessageBox.Show("Order approved and set as Available.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadPendingOrders();
    }

    private void DeleteOrderBtn(object sender, RoutedEventArgs e)
    {
        if (PendingOrdersList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select order");
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

        var client = db.Clients.FirstOrDefault(c => c.Id == task.ClientId);

        if (client != null)
            client.CountOrders--;

        db.Tasks.Remove(task);

        db.SaveChanges();

        MessageBox.Show("Order deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadPendingOrders();
    }
}
