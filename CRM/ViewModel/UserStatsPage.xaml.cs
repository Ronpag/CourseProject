using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class UserStatsPage : Page
{
    private readonly int _userId;

    public UserStatsPage(int userId)
    {
        InitializeComponent();
        _userId = userId;
        LoadStats(null, null);
    }

    private void LoadStats(DateTime? from, DateTime? to)
    {
        var user = UserService.GetById(_userId);
        RegistrationDate.Text = user != null
            ? $"Registered: {user.RegistrationDate:dd.MM.yyyy HH:mm}"
            : "";

        var counts = StatsService.GetTaskCounts(userId: _userId, from: from, to: to);
        int total = counts.Values.Sum();
        int pendingReview = counts[Data.Task.TaskStatus.Pending] +
                            counts[Data.Task.TaskStatus.Available];

        TotalTasks.Text = $"Total Tasks: {total}";
        AssignedTasks.Text = $"Assigned: {counts[Data.Task.TaskStatus.Assigned]}";
        InProgressTasks.Text = $"In Progress: {counts[Data.Task.TaskStatus.InProgress]}";
        CompletedTasks.Text = $"Completed: {counts[Data.Task.TaskStatus.Completed]}";
        PendingReview.Text = $"Pending / Available: {pendingReview}";
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
