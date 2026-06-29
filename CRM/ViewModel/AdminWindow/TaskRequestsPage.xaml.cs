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
    }

    private void LoadRequests()
    {
        using var db = new AppDbContext();

        RequestsList.ItemsSource = db.TaskStatusRequests
            .Where(r => !r.IsProcessed)
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

        dbRequest.IsProcessed = true;
        dbRequest.IsApproved = true;

        db.SaveChanges();

        MessageBox.Show(
            "Status request approved.",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

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

        MessageBox.Show(
            "Status request rejected.",
            "Rejected",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        LoadRequests();
    }
}