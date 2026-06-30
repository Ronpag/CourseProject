using System.Windows;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class ChangeTaskStatusWindow : Window
{
    private readonly int _taskId;

    public ChangeTaskStatusWindow(CRM.Data.Task task)
    {
        InitializeComponent();

        _taskId = task.Id;

        StatusBox.ItemsSource =
            Enum.GetValues(typeof(CRM.Data.Task.TaskStatus));

        StatusBox.SelectedItem = task.Status;
    }

    private void SaveBtn(object sender, RoutedEventArgs e)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == _taskId);

        if (task == null)
            return;

        db.TaskStatusRequests.Add(new TaskStatusRequest
        {
            TaskId = _taskId,
            RequestedStatus =
                (CRM.Data.Task.TaskStatus)StatusBox.SelectedItem,
            Comment = CommentBox.Text.Trim(),
            IsProcessed = false
        });

        db.SaveChanges();

        MessageBox.Show("Request has been sent to administrator.", "Success");

        DialogResult = true;
        Close();
    }
}
