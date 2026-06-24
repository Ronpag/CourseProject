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

        NameTaskBox.Text = task.NameTask;
        ClientIdBox.Text = task.ClientId.ToString();
        WorkerIdBox.Text = task.WorkerId.ToString();
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ClientIdBox.Text, out int clientId))
        {
            MessageBox.Show("Invalid Client Id");
            return;
        }

        if (!int.TryParse(WorkerIdBox.Text, out int workerId))
        {
            MessageBox.Show("Invalid Worker Id");
            return;
        }

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == _taskId);

        if (task == null)
            return;

        task.NameTask = NameTaskBox.Text.Trim();
        task.ClientId = clientId;
        task.WorkerId = workerId;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}