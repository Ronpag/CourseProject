using System.Windows.Controls;

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
        var counts = StatsService.GetTaskCounts(clientId: _clientId);

        int total = counts.Values.Sum();

        TotalOrders.Text = $"Total Orders: {total}";
        PendingOrders.Text = $"Pending: {counts[CRM.Data.Task.TaskStatus.Pending]}";
        AvailableOrders.Text = $"Available: {counts[CRM.Data.Task.TaskStatus.Available]}";
        AssignedOrders.Text = $"Assigned: {counts[CRM.Data.Task.TaskStatus.Assigned]}";
        InProgressOrders.Text = $"In Progress: {counts[CRM.Data.Task.TaskStatus.InProgress]}";
        CompletedOrders.Text = $"Completed: {counts[CRM.Data.Task.TaskStatus.Completed]}";
    }
}
