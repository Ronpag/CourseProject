using System.Linq;
using System.Windows.Controls;

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
        var counts = StatsService.GetTaskCounts();
        int totalTasks = counts.Values.Sum();

        TotalUsers.Text = $"Total Users: {StatsService.GetUserCount()}";
        TotalClients.Text = $"Total Clients: {StatsService.GetClientCount()}";
        TotalTasks.Text = $"Total Tasks: {totalTasks}";
        PendingOrders.Text = $"Pending Orders: {counts[CRM.Data.Task.TaskStatus.Pending]}";
        AvailableTasks.Text = $"Available Tasks: {counts[CRM.Data.Task.TaskStatus.Available]}";
        AssignedTasks.Text = $"Assigned Tasks: {counts[CRM.Data.Task.TaskStatus.Assigned]}";
        InProgressTasks.Text = $"In Progress Tasks: {counts[CRM.Data.Task.TaskStatus.InProgress]}";
        CompletedTasks.Text = $"Completed Tasks: {counts[CRM.Data.Task.TaskStatus.Completed]}";
        PendingRequests.Text = $"Pending Status Requests: {StatsService.GetPendingStatusRequestsCount()}";
        ProfileRequests.Text = $"Pending Profile Requests: {StatsService.GetPendingProfileRequestsCount()}";
    }
}
