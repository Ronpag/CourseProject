using System.Linq;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.ClientWindow;

public partial class ClientStatsPage : Page
{
    private readonly int _clientId;

    public ClientStatsPage(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;
        LoadStats();
    }

    private void LoadStats()
    {
        using var db = new AppDbContext();

        var tasks = db.Tasks.Where(t => t.ClientId == _clientId).ToList();

        int total = tasks.Count;
        int pending = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Pending);
        int available = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Available);
        int assigned = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Assigned);
        int inProgress = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.InProgress);
        int completed = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Completed);

        TotalOrders.Text = $"Total Orders: {total}";
        PendingOrders.Text = $"Pending: {pending}";
        AvailableOrders.Text = $"Available: {available}";
        AssignedOrders.Text = $"Assigned: {assigned}";
        InProgressOrders.Text = $"In Progress: {inProgress}";
        CompletedOrders.Text = $"Completed: {completed}";
    }
}
