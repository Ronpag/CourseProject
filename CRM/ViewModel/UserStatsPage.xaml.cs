using System.Windows.Controls;

namespace CRM.ViewModel.UserWindow;

public partial class UserStatsPage : Page
{
    private readonly int _userId;

    public UserStatsPage(int userId)
    {
        InitializeComponent();
        _userId = userId;
        LoadStats();
    }

    private void LoadStats()
    {
        var counts = StatsService.GetTaskCounts(userId: _userId);

        int total = counts.Values.Sum();
        int pendingReview = counts[CRM.Data.Task.TaskStatus.Pending] +
                            counts[CRM.Data.Task.TaskStatus.Available];

        TotalTasks.Text = $"Total Tasks: {total}";
        AssignedTasks.Text = $"Assigned: {counts[CRM.Data.Task.TaskStatus.Assigned]}";
        InProgressTasks.Text = $"In Progress: {counts[CRM.Data.Task.TaskStatus.InProgress]}";
        CompletedTasks.Text = $"Completed: {counts[CRM.Data.Task.TaskStatus.Completed]}";
        PendingReview.Text = $"Pending / Available: {pendingReview}";
    }
}
