using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class TaskUpdateWindow : Window
{
    private readonly int _taskId;

    public TaskUpdateWindow(CRM.Data.Task task)
    {
        InitializeComponent();

        _taskId = task.Id;

        TaskNameBox.Text = task.TaskName;
        DescriptionBox.Text = task.Description;
        ClientIdBox.Text = task.ClientId.ToString();
        UserIdBox.Text = task.UserId?.ToString() ?? "";

        StatusBox.ItemsSource =
            Enum.GetValues(typeof(CRM.Data.Task.TaskStatus));

        StatusBox.SelectedItem = task.Status;

        StartDateBox.Text = task.StartDate?.ToString("dd.MM.yyyy") ?? "";
        AcceptanceDateBox.Text = task.AcceptanceDate?.ToString("dd.MM.yyyy") ?? "";
        CompletionDateBox.Text = task.CompletionDate?.ToString("dd.MM.yyyy") ?? "";
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        string taskName = TaskNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(taskName))
        {
            MessageBox.Show("Task name is empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ValidationService.ValidateEnglishText(taskName, "Task name"))
            return;

        string description = DescriptionBox.Text.Trim();
        if (!string.IsNullOrWhiteSpace(description) && !ValidationService.IsEnglishText(description))
        {
            MessageBox.Show("Description must contain only English characters", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(ClientIdBox.Text, out int clientId))
        {
            MessageBox.Show("Invalid Client Id", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var status = (CRM.Data.Task.TaskStatus)StatusBox.SelectedItem;

        int? UserId = null;

        if (status != CRM.Data.Task.TaskStatus.Available && status != CRM.Data.Task.TaskStatus.Pending)
        {
            if (!int.TryParse(UserIdBox.Text, out int parsedUserId))
            {
                MessageBox.Show("Invalid User Id", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserId = parsedUserId;
        }

        DateTime? startDate = null;
        if (!string.IsNullOrWhiteSpace(StartDateBox.Text))
        {
            if (DateTime.TryParse(StartDateBox.Text, out var parsedStart))
                startDate = parsedStart;
            else
            {
                MessageBox.Show("Invalid Start Date", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        DateTime? acceptanceDate = null;
        if (!string.IsNullOrWhiteSpace(AcceptanceDateBox.Text))
        {
            if (DateTime.TryParse(AcceptanceDateBox.Text, out var parsedAccept))
                acceptanceDate = parsedAccept;
            else
            {
                MessageBox.Show("Invalid Acceptance Date", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        DateTime? completionDate = null;
        if (!string.IsNullOrWhiteSpace(CompletionDateBox.Text))
        {
            if (DateTime.TryParse(CompletionDateBox.Text, out var parsedComplete))
                completionDate = parsedComplete;
            else
            {
                MessageBox.Show("Invalid Completion Date", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == _taskId);

        if (task == null)
        {
            MessageBox.Show("Task not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var client = db.Clients.FirstOrDefault(c => c.Id == clientId);

        if (client == null)
        {
            MessageBox.Show("Client with this ID does not exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (UserId != null)
        {
            var User = db.Users.FirstOrDefault(u => u.Id == UserId);

            if (User == null)
            {
                MessageBox.Show("User with this ID does not exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        if (startDate.HasValue && acceptanceDate.HasValue && acceptanceDate < startDate)
        {
            MessageBox.Show("Acceptance date cannot be earlier than start date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (acceptanceDate.HasValue && completionDate.HasValue && completionDate < acceptanceDate)
        {
            MessageBox.Show("Completion date cannot be earlier than acceptance date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (task.ClientId != clientId)
        {
            var oldClient = db.Clients.FirstOrDefault(c => c.Id == task.ClientId);

            if (oldClient != null && oldClient.CountOrders > 0)
                oldClient.CountOrders--;

            client.CountOrders++;
        }

        task.TaskName = taskName;
        task.Description = DescriptionBox.Text.Trim();
        task.ClientId = clientId;
        task.UserId = UserId;
        task.Status = status;
        task.StartDate = startDate;
        task.AcceptanceDate = acceptanceDate;
        task.CompletionDate = completionDate;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}
