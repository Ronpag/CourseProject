using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class FSWindow : Window
{
    public FSWindow()
    {
        InitializeComponent();
    }
    
    private void FirstLoginBtn(object sender, RoutedEventArgs e)
    {
        string login = FLogin.Text?.Trim() ?? "";
        string password = FPassword.Text?.Trim() ?? "";

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        db.Users.Add(new User
        {
            Name = login,
            Password = password,
            IsAdmin = true,
            IsActive = true
        });

        db.SaveChanges();

        new LoginWindow().Show();
        Close();
    }
}