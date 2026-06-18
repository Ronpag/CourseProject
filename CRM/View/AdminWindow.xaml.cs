using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
    }
    
    private void BackBtn(object sender, RoutedEventArgs e)
    {
        LoginWindow login = new LoginWindow();
        login.Show();
        
        Window currentWindow = Window.GetWindow(this);
        currentWindow?.Close();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string login = Login.Text?.Trim() ?? "";
        string password = Password.Text?.Trim() ?? "";

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Empty login or password");
            return;
        }

        if (login.Length < 3 || password.Length < 3)
        {
            Console.WriteLine("Small password or login");
            return;
        }
        
        if (login.Length < 3 || password.Length < 3)
        {
            Console.WriteLine("Small password or login");
            return;
        }
        
        using var db = new AppDbContext();
        
        bool userExists = db.Users.Any(u => u.Name == login);

        if (userExists)
        {
            Console.WriteLine("User already exists");
            return;
        }

        var user = new User
        {
            Name = login,
            Password = password,
            IsAdmin = false
        };

        db.Users.Add(user);
        db.SaveChanges();

        Console.WriteLine("User registered");
    }
}