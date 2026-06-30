using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

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
        if (TasksList == null) return;

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

        if (task.StartDate.HasValue && DateTime.Now < task.StartDate.Value)
        {
            MessageBox.Show("Task cannot be accepted before its start date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        task.WorkerId = _workerId;
        task.Status = CRM.Data.Task.TaskStatus.Assigned;
        task.AcceptanceDate = DateTime.Now;

        db.SaveChanges();

        LoadTasks();

        MessageBox.Show("Task accepted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }

    private void TasksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }
}
