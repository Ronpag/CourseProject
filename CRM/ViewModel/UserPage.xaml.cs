using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.ViewModel.UserWindow;

public partial class UserPage : Page
{
    private readonly int _UserId;

    public UserPage(int UserId)
    {
        InitializeComponent();
        _UserId = UserId;
        LoadTasks();
    }

    private void LoadTasks()
    {
        if (TasksList == null) return;
        var statusFilters = new List<CRM.Data.Task.TaskStatus>();
        if (ChkAssigned.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Assigned);
        if (ChkInProgress.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.InProgress);
        if (ChkCompleted.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Completed);
        TasksList.ItemsSource = TaskService.GetFiltered(UserId: _UserId, statusFilters: statusFilters);
    }

    private void FilterChanged(object sender, RoutedEventArgs e) => LoadTasks();

    private void ChangeStatusBtn(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not CRM.Data.Task task)
        {
            MessageBox.Show("Select task");
            return;
        }
        var window = new ChangeTaskStatusWindow(task);
        if (window.ShowDialog() == true)
            LoadTasks();
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