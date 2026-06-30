using System.Windows;

namespace CRM.ViewModel.ClientWindow;

public partial class CreateOrderWindow : Window
{
    private readonly int _clientId;

    public CreateOrderWindow(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string orderName = OrderNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(orderName))
        {
            MessageBox.Show("Enter order name");
            return;
        }

        if (TaskService.Create(orderName, DescriptionBox.Text.Trim(),
                _clientId, null,
                CRM.Data.Task.TaskStatus.Pending,
                DateTime.Now, null))
        {
            MessageBox.Show("Order created and sent for moderation.", "Success");
            DialogResult = true;
            Close();
        }
    }
}
