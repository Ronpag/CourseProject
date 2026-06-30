using System.Windows;
using CRM.Data;

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

        using var db = new AppDbContext();

        var client = db.Clients.FirstOrDefault(c => c.Id == _clientId);

        if (client == null)
        {
            MessageBox.Show("Client not found");
            return;
        }

        db.Tasks.Add(new CRM.Data.Task
        {
            TaskName = orderName,
            Description = DescriptionBox.Text.Trim(),
            ClientId = client.Id,
            WorkerId = null,
            Status = CRM.Data.Task.TaskStatus.Pending,
            StartDate = DateTime.Now
        });

        client.CountOrders++;

        db.SaveChanges();

        MessageBox.Show("Order created and sent for moderation.", "Success");
        DialogResult = true;
        Close();
    }
}
