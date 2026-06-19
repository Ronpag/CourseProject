using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class RegistrationPage : Page
{
    public RegistrationPage()
    {
        InitializeComponent();
    }
    
    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string login = Login.Text?.Trim() ?? "";
        string password = Password.Text?.Trim() ?? "";

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show(
                "The password or login is empty",
                "Empty",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);   
            return;
        }

        if (login.Length <= 3 || password.Length <= 3)
        {
            MessageBox.Show(
                "The password or login is too short",
                "Short",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);   
            return;
        }
        
        using var db = new AppDbContext();
        
        bool userExists = db.Users.Any(u => u.Name == login);

        if (userExists)
        {
            MessageBox.Show(
                "This login is exists",
                "Login",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);   
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

        MessageBox.Show(
            "User was created",
            "Sucscess",
            MessageBoxButton.OK,
            MessageBoxImage.Information);   
    }
}