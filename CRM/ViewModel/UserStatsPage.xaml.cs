using System.Linq;
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
        LoadStats();
    }

    private void LoadStats()
    {
        using var db = new AppDbContext();

        var tasks = db.Tasks.Where(t => t.WorkerId == _userId).ToList();

        int total = tasks.Count;
        int assigned = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Assigned);
        int inProgress = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.InProgress);
        int completed = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Completed);
        int pendingReview = tasks.Count(t =>
            t.Status == CRM.Data.Task.TaskStatus.Pending ||
            t.Status == CRM.Data.Task.TaskStatus.Available);

        TotalTasks.Text = $"Total Tasks: {total}";
        AssignedTasks.Text = $"Assigned: {assigned}";
        InProgressTasks.Text = $"In Progress: {inProgress}";
        CompletedTasks.Text = $"Completed: {completed}";
        PendingReview.Text = $"Pending / Available: {pendingReview}";
    }
}
