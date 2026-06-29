using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class RegisterClientWindow : Window
{
    public RegisterClientWindow()
    {
        InitializeComponent();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string name = ClientNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show(
                "Client name is empty",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        using var db = new AppDbContext();

        if (db.Clients.Any(c => c.NameClient == name))
        {
            MessageBox.Show(
                "This client already exists",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

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