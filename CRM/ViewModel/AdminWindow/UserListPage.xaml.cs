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
            MessageBox.Show("Login already exists");
            return;
        }

        db.Users.Add(new User
        {
            Name = login,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            IsAdmin = false,
            IsActive = true
        });

        db.SaveChanges();

        LoadUsers();
        MessageBox.Show("User created");
    }

    private void DeleteBtn(object sender, RoutedEventArgs e)
    {
        if (UsersList.SelectedItem is not User selectedUser)
        {
            MessageBox.Show("Select user");
            return;
        }

        using var db = new AppDbContext();

        var user = db.Users.FirstOrDefault(u => u.Id == selectedUser.Id);

        if (user == null) return;

        if (user.IsAdmin)
        {
            MessageBox.Show("Cannot delete admin");
            return;
        }

        db.Users.Remove(user);
        db.SaveChanges();

        LoadUsers();
    }

    private void UpdateBtn(object sender, RoutedEventArgs e)
    {
        if (UsersList.SelectedItem is not User selectedUser)
        {
            MessageBox.Show("Select user");
            return;
        }

        var window = new UserUpdateWindow(selectedUser);

        if (window.ShowDialog() == true)
            LoadUsers();
    }

    private void LoadUsers()
    {
        using var db = new AppDbContext();
        UsersList.ItemsSource = db.Users.ToList();
    }
}