using System.Windows;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class CreateClientWindow : Window
{
    public CreateClientWindow()
    {
        InitializeComponent();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string name = ClientNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Enter client name");
            return;
        }

        using var db = new AppDbContext();

        if (db.Clients.Any(c => c.NameClient == name))
        {
            MessageBox.Show("Client already exists");
            return;
        }

        db.Clients.Add(new Client
        {
            NameClient = name,
            CountOrders = 0
        });

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}