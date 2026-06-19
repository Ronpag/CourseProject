using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginBtn(object sender, RoutedEventArgs e)
    {
        string login = Login.Text?.Trim() ?? "";
        string password = Password.Text?.Trim() ?? "";

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show(
                "Empty login or password",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);   
            return;
        }

        using var db = new AppDbContext();

        var user = db.Users
            .FirstOrDefault(u => u.Name == login && u.Password == password);

        if (user == null)
        {
            MessageBox.Show(
                "Invalid login or password",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);   
            return;
        }
        
        Console.WriteLine($"Login success: {user.Name} (Id: {user.Id})");

        Window window;

        if (user.IsAdmin)
        {
            window = new AdminWindow();
        }
        else
        {
            window = new UserWindow();
        }

        window.Show();

        Window currentWindow = Window.GetWindow(this);
        currentWindow?.Close();
    }
}