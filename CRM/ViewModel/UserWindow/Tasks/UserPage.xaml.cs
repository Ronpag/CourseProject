using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.ViewModel.UserWindow;

public partial class UserPage : Page
{
    private readonly int _workerId;

    public UserPage(int workerId)
    {
        InitializeComponent();

        _workerId = workerId;

        LoadTasks();
    }

    private void LoadTasks()
    {
        using var db = new AppDbContext();

        var query = db.Tasks.Where(t => t.WorkerId == _workerId);

        string filter = SearchBox?.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(t => t.TaskName.Contains(filter));

        TasksList.ItemsSource = query.ToList();
    }

    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        LoadTasks();
    }

    private void ChangeStatusBtn(object sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task task)
        {
            MessageBox.Show("Select task");
            return;
        }

        var window = new ChangeTaskStatusWindow(task);

        if (window.ShowDialog() == true)
        {
            LoadTasks();
        }
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