using System.Windows;
using System.Windows.Controls;
using CRM.Data;

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

        TasksList.ItemsSource = db.Tasks
            .Where(t => t.WorkerId == _workerId)
            .ToList();
    }

    private void AddClientBtn(object sender, RoutedEventArgs e)
    {
        var window = new CreateClientWindow();

        if (window.ShowDialog() == true)
        {
            MessageBox.Show("Client created");
        }
    }

    private void AddTaskBtn(object sender, RoutedEventArgs e)
    {
        var window = new CreateTaskWindow(_workerId);

        if (window.ShowDialog() == true)
        {
            LoadTasks();
        }
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
}