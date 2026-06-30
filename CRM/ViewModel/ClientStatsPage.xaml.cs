using System.Windows;
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
        LoadStats(null, null);
    }

    private void LoadStats(DateTime? from, DateTime? to)
    {
        var counts = StatsService.GetTaskCounts(clientId: _clientId, from: from, to: to);
        int total = counts.Values.Sum();

        TotalOrders.Text = $"Total Orders: {total}";
        PendingOrders.Text = $"Pending: {counts[Data.Task.TaskStatus.Pending]}";
        AvailableOrders.Text = $"Available: {counts[Data.Task.TaskStatus.Available]}";
        AssignedOrders.Text = $"Assigned: {counts[Data.Task.TaskStatus.Assigned]}";
        InProgressOrders.Text = $"In Progress: {counts[Data.Task.TaskStatus.InProgress]}";
        CompletedOrders.Text = $"Completed: {counts[Data.Task.TaskStatus.Completed]}";
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
