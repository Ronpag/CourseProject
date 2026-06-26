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
        
        if (!user.IsActive)
        {
            MessageBox.Show(
                "Your account is disabled. Please contact administrator.",
                "Access denied",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
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
            window = new UserWindow(user.Id);
        }

        window.Show();

        Window currentWindow = Window.GetWindow(this);
        currentWindow?.Close();
    }
}