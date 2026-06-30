using System.Windows;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class CreateTaskWindow : Window
{
    private readonly int _workerId;

    public CreateTaskWindow(int workerId)
    {
        InitializeComponent();

        _workerId = workerId;

        ClientsBox.ItemsSource = ClientService.GetAll();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string taskName = TaskNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(taskName))
        {
            MessageBox.Show("Enter task name");
            return;
        }

        if (ClientsBox.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show("Select client");
            return;
        }

        if (TaskService.Create(taskName, DescriptionBox.Text.Trim(),
                selectedClient.Id, _workerId,
                CRM.Data.Task.TaskStatus.Assigned,
                DateTime.Now, DateTime.Now))
        {
            DialogResult = true;
            Close();
        }
    }
}
