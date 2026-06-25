using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class WorkerPage : Page
{
    private readonly int _workerId;

    public WorkerPage(int workerId)
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
}