using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.ViewModel.UserWindow;

public partial class AvailableTasksPage : Page
{
    private readonly int _UserId;

    public AvailableTasksPage(int UserId)
    {
        InitializeComponent();
        _UserId = UserId;
        LoadTasks();
    }

    private void LoadTasks()
    {
        if (TasksList == null) return;
        TasksList.ItemsSource = TaskService.GetAvailable();
    }

    private void TakeTaskBtn(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select task");
            return;
        }
        if (TaskService.TakeTask(selectedTask.Id, _UserId))
        {
            LoadTasks();
            MessageBox.Show("Task accepted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }

    private void TasksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }
}