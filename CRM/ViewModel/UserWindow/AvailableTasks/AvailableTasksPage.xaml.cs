using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class AvailableTasksPage : Page
{
    private readonly int _workerId;

    public AvailableTasksPage(int workerId)
    {
        InitializeComponent();

        _workerId = workerId;

        LoadTasks();
    }

    private void LoadTasks()
    {
        using var db = new AppDbContext();

        TasksList.ItemsSource = db.Tasks
            .Where(t => t.Status == CRM.Data.Task.TaskStatus.Available)
            .ToList();
    }

    private void TakeTaskBtn(object sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select task");
            return;
        }

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == selectedTask.Id);

        if (task == null)
            return;

        task.WorkerId = _workerId;
        task.Status = CRM.Data.Task.TaskStatus.Assigned;

        db.SaveChanges();

        LoadTasks();

        MessageBox.Show(
            "Task accepted",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}