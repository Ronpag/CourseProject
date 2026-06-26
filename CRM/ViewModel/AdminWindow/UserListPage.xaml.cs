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

        if (!Validation.ValidateLoginPassword(login, password))
            return;

        using var db = new AppDbContext();

        if (db.Users.Any(u => u.Name == login))
        {
            MessageBox.Show(
                "This login already exists",
                "Login",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        db.Users.Add(new User
        {
            Name = login,
            Password = password,
            IsAdmin = false,
            IsActive = true
        });

        db.SaveChanges();

        LoadUsers();

        MessageBox.Show(
            "User was created",
            "Success",
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
        
        if (user.IsAdmin)
        {
            MessageBox.Show(
                "You cannot delete an admin account",
                "Access denied",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            return;
        }

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