using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class UserListPage : Page
{
    public UserListPage()
    {
        InitializeComponent();
        LoadUsers();
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

        if (!Validation.IsEnglish(login) || !Validation.IsEnglish(password))
        {
            MessageBox.Show(
                "Login and password must contain only English letters and numbers",
                "Invalid characters",
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
        
        LoadUsers();

        MessageBox.Show(
            "User was created",
            "Sucscess",
            MessageBoxButton.OK,
            MessageBoxImage.Information);   
    }
    
    private void DeleteBtn(object sender, RoutedEventArgs e)
    {
        if (UsersList.SelectedItem is not User selectedUser)
        {
            MessageBox.Show(
                "Select a user",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(u => u.Id == selectedUser.Id);

        if (user == null)
            return;

        db.Users.Remove(user);
        db.SaveChanges();

        LoadUsers();

        MessageBox.Show(
            "User deleted",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
    
    private void UpdateBtn(object sender, RoutedEventArgs e)
    {
        if (UsersList.SelectedItem is not User selectedUser)
        {
            MessageBox.Show(
                "Select a user",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var window = new UserUpdateWindow(selectedUser);

        bool? result = window.ShowDialog();

        if (result == true)
        {
            LoadUsers();

            MessageBox.Show(
                "User updated",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
    
    private void LoadUsers()
    {
        using var db = new AppDbContext();

        UsersList.ItemsSource = db.Users.ToList();
    }
}