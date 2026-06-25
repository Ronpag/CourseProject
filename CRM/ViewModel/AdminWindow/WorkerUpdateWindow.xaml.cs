using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class WorkerUpdateWindow : Window
{
    private readonly int _userId;

    public WorkerUpdateWindow(User user)
    {
        InitializeComponent();

        _userId = user.Id;

        LoginBox.Text = user.Name;
        PasswordBox.Text = user.Password;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
    {
        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(x => x.Id == _userId);

        if (user == null)
            return;

        user.Name = LoginBox.Text.Trim();
        user.Password = PasswordBox.Text.Trim();

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}