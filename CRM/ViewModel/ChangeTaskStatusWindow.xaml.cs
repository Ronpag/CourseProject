using System;
using System.Windows;

namespace CRM.ViewModel.UserWindow;

public partial class ChangeTaskStatusWindow : Window
{
    private readonly int _taskId;

    public ChangeTaskStatusWindow(CRM.Data.Task task)
    {
        InitializeComponent();

        _taskId = task.Id;

        StatusBox.ItemsSource =
            Enum.GetValues(typeof(CRM.Data.Task.TaskStatus));

        StatusBox.SelectedItem = task.Status;
    }

    private void StatusBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        CompletionDatePanel.Visibility =
            StatusBox.SelectedItem is CRM.Data.Task.TaskStatus status && status == CRM.Data.Task.TaskStatus.Completed
                ? Visibility.Visible
                : Visibility.Collapsed;
    }

    private void SaveBtn(object sender, RoutedEventArgs e)
    {
        var requestedStatus = (CRM.Data.Task.TaskStatus)StatusBox.SelectedItem;

        DateTime? completionDate = null;

        if (requestedStatus == CRM.Data.Task.TaskStatus.Completed)
        {
            if (!string.IsNullOrWhiteSpace(CompletionDateBox.Text))
            {
                if (!DateTime.TryParse(CompletionDateBox.Text, out var parsed))
                {
                    MessageBox.Show("Invalid completion date", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                completionDate = parsed;
            }
        }

        if (TaskService.ChangeStatus(_taskId, requestedStatus, CommentBox.Text.Trim(), completionDate))
        {
            MessageBox.Show("Request has been sent to administrator.", "Success");
            DialogResult = true;
            Close();
        }
    }
}
