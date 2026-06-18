using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class FSWindow : Page
{
    public FSWindow()
    {
        InitializeComponent();
    }
    
    private void FirstLoginBtn(object sender, RoutedEventArgs e)
    {
        string login = FLogin.Text?.Trim() ?? "";
        string password = FPassword.Text?.Trim() ?? "";

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
        
        using var db = new AppDbContext();

        var user = new User
        {
            Id = 1,
            Name = login,
            Password = password,
            IsAdmin = true
        };

        db.Users.Add(user);
        db.SaveChanges();

        Console.WriteLine("User saved");
        
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();
        
        Window currentWindow = Window.GetWindow(this);
        currentWindow?.Close();
    }
}