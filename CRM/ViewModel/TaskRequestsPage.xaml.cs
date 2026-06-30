using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.ViewModel.AdminWindow;

public partial class TaskRequestsPage : Page
{
    public TaskRequestsPage()
    {
        InitializeComponent();
        LoadPendingOrders();
        LoadProfileRequests();
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
        {
            DateTime completionDate = dbRequest.RequestedCompletionDate ?? DateTime.Now;

            if (task.AcceptanceDate.HasValue && completionDate < task.AcceptanceDate.Value)
            {
                MessageBox.Show("Completion date cannot be earlier than acceptance date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            task.CompletionDate = completionDate;
        }

        dbRequest.IsProcessed = true;
        dbRequest.IsApproved = true;

        db.SaveChanges();

        MessageBox.Show("Status request approved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

    private void OpenRequestDetails(TaskStatusRequest request)
    {
        if (request == null) return;
        new DetailsWindow(request).ShowDialog();
    }

    private void OpenOrderDetails(CRM.Data.Task task)
    {
        if (task == null) return;
        new DetailsWindow(task).ShowDialog();
    }

    private void RequestDetailsBtn(object sender, RoutedEventArgs e)
    {
        OpenRequestDetails(RequestsList.SelectedItem as TaskStatusRequest);
    }

    private void RequestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OpenRequestDetails(RequestsList.SelectedItem as TaskStatusRequest);
    }

    private void OrderDetailsBtn(object sender, RoutedEventArgs e)
    {
        OpenOrderDetails(PendingOrdersList.SelectedItem as CRM.Data.Task);
    }

    private void PendingOrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OpenOrderDetails(PendingOrdersList.SelectedItem as CRM.Data.Task);
    }

    private void LoadProfileRequests()
    {
        using var db = new AppDbContext();

        ProfileRequestsList.ItemsSource = db.ProfileChangeRequests
            .Where(r => !r.IsProcessed)
            .ToList();
    }

    private void ApproveProfileBtn(object sender, RoutedEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is not ProfileChangeRequest request)
        {
            MessageBox.Show("Select a request");
            return;
        }

        if (request.NewEmail != null && !Validation.ValidateEmail(request.NewEmail))
            return;

        if (request.NewPhone != null && !Validation.ValidatePhone(request.NewPhone))
            return;

        using var db = new AppDbContext();

        var dbRequest = db.ProfileChangeRequests
            .FirstOrDefault(r => r.Id == request.Id);

        if (dbRequest == null) return;

        if (dbRequest.UserId != null)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == dbRequest.UserId);

            if (user == null)
            {
                MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dbRequest.NewEmail != null) user.Email = dbRequest.NewEmail;
            if (dbRequest.NewPhone != null) user.Phone = dbRequest.NewPhone;
            if (dbRequest.NewPosition != null) user.Position = dbRequest.NewPosition;
        }
        else if (dbRequest.ClientId != null)
        {
            var client = db.Clients.FirstOrDefault(c => c.Id == dbRequest.ClientId);

            if (client == null)
            {
                MessageBox.Show("Client not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dbRequest.NewEmail != null) client.Email = dbRequest.NewEmail;
            if (dbRequest.NewPhone != null) client.Phone = dbRequest.NewPhone;
            if (dbRequest.NewAddress != null) client.Address = dbRequest.NewAddress;
        }

        dbRequest.IsProcessed = true;
        dbRequest.IsApproved = true;

        db.SaveChanges();

        MessageBox.Show("Profile changes approved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadProfileRequests();
    }

    private void RejectProfileBtn(object sender, RoutedEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is not ProfileChangeRequest request)
        {
            MessageBox.Show("Select a request");
            return;
        }

        using var db = new AppDbContext();

        var dbRequest = db.ProfileChangeRequests
            .FirstOrDefault(r => r.Id == request.Id);

        if (dbRequest == null) return;

        dbRequest.IsProcessed = true;
        dbRequest.IsApproved = false;

        db.SaveChanges();

        MessageBox.Show("Profile changes rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Information);

        LoadProfileRequests();
    }

    private void ProfileDetailsBtn(object sender, RoutedEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is not ProfileChangeRequest request) return;
        new DetailsWindow(request).ShowDialog();
    }

    private void ProfileRequestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is not ProfileChangeRequest request) return;
        new DetailsWindow(request).ShowDialog();
    }
}
