using System.Linq;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.AdminWindow;

public partial class StatsPage : Page
{
    public StatsPage()
    {
        InitializeComponent();
        LoadStats();
    }

    private void LoadStats()
    {
        using var db = new AppDbContext();

        int totalUsers = db.Users.Count();
        int totalClients = db.Clients.Count();
        int totalTasks = db.Tasks.Count();

        int pendingOrders = db.Tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Pending);
        int availableTasks = db.Tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Available);
        int assignedTasks = db.Tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Assigned);
        int inProgressTasks = db.Tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.InProgress);
        int completedTasks = db.Tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Completed);

        int pendingStatusRequests = db.TaskStatusRequests.Count(r => !r.IsProcessed);
        int pendingProfileRequests = db.ProfileChangeRequests.Count(r => !r.IsProcessed);
        int workerCount = db.Users.Count(u => !u.IsAdmin);

        TotalUsers.Text = $"Total Workers: {totalUsers}";
        TotalClients.Text = $"Total Clients: {totalClients}";
        TotalTasks.Text = $"Total Tasks: {totalTasks}";
        PendingOrders.Text = $"Pending Orders: {pendingOrders}";
        AvailableTasks.Text = $"Available Tasks: {availableTasks}";
        AssignedTasks.Text = $"Assigned Tasks: {assignedTasks}";
        InProgressTasks.Text = $"In Progress Tasks: {inProgressTasks}";
        CompletedTasks.Text = $"Completed Tasks: {completedTasks}";
        PendingRequests.Text = $"Pending Status Requests: {pendingStatusRequests}";
        ProfileRequests.Text = $"Pending Profile Requests: {pendingProfileRequests}";
        WorkerCount.Text = $"Active Workers: {workerCount}";
    }
}
