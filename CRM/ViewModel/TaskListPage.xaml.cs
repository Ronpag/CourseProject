using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.View;

public partial class TaskListPage : Page
{
    public TaskListPage()
    {
        InitializeComponent();
        LoadTasks();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        var window = new CreateTaskWindow();
        if (window.ShowDialog() == true)
        {
            LoadTasks();
            MessageBox.Show("Task created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void DeleteBtn(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select task");
            return;
        }
        if (TaskService.Delete(selectedTask.Id))
        {
            LoadTasks();
            MessageBox.Show("Task deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void UpdateBtn(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select task", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var window = new TaskUpdateWindow(selectedTask);
        if (window.ShowDialog() == true)
        {
            LoadTasks();
            MessageBox.Show("Task updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void LoadTasks()
    {
        if (TasksList == null) return;
        var statusFilters = new List<CRM.Data.Task.TaskStatus>();
        if (ChkPending.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Pending);
        if (ChkAvailable.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Available);
        if (ChkAssigned.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Assigned);
        if (ChkInProgress.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.InProgress);
        if (ChkCompleted.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Completed);
        TasksList.ItemsSource = TaskService.GetFiltered(
            statusFilters: statusFilters,
            dateFrom: DateFrom?.SelectedDate,
            dateTo: DateTo?.SelectedDate);
    }

    private void FilterChanged(object sender, RoutedEventArgs e) => LoadTasks();
    private void DateFilterChanged(object sender, SelectionChangedEventArgs e) => LoadTasks();

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