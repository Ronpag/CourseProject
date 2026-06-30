using System.Windows;
using System.Windows.Controls;

namespace CRM.ViewModel.AdminWindow;

public partial class StatsPage : Page
{
    public StatsPage()
    {
        InitializeComponent();
        LoadStats(null, null);
    }

    private void LoadStats(DateTime? from, DateTime? to)
    {
        var counts = StatsService.GetTaskCounts(from: from, to: to);
        int total = counts.Values.Sum();

        TotalUsers.Text = $"Total Users: {StatsService.GetUserCount()}";
        TotalClients.Text = $"Total Clients: {StatsService.GetClientCount()}";
        TotalTasks.Text = $"Total Tasks: {total}";
        PendingOrders.Text = $"Pending Orders: {counts[Data.Task.TaskStatus.Pending]}";
        AvailableTasks.Text = $"Available Tasks: {counts[Data.Task.TaskStatus.Available]}";
        AssignedTasks.Text = $"Assigned Tasks: {counts[Data.Task.TaskStatus.Assigned]}";
        InProgressTasks.Text = $"In Progress Tasks: {counts[Data.Task.TaskStatus.InProgress]}";
        CompletedTasks.Text = $"Completed Tasks: {counts[Data.Task.TaskStatus.Completed]}";
        PendingRequests.Text = $"Pending Status Requests: {StatsService.GetPendingStatusRequestsCount()}";
        ProfileRequests.Text = $"Pending Profile Requests: {StatsService.GetPendingProfileRequestsCount()}";
    }

    private void ApplyFilter(object sender, RoutedEventArgs e)
    {
        LoadStats(DateFrom.SelectedDate, DateTo.SelectedDate);
    }

    private void ResetFilter(object sender, RoutedEventArgs e)
    {
        DateFrom.SelectedDate = null;
        DateTo.SelectedDate = null;
        LoadStats(null, null);
    }
}
