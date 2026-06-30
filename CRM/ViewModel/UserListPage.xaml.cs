using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

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
        var window = new RegisterUserWindow();

        if (window.ShowDialog() == true)
        {
            LoadUsers();
        }
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
        if (UsersList == null) return;

        using var db = new AppDbContext();

        var query = db.Users.AsQueryable();

        if (ChkWorkers.IsChecked == true)
            query = query.Where(u => !u.IsAdmin);
        if (ChkAdmins.IsChecked == true)
            query = query.Where(u => u.IsAdmin);
        if (ChkActive.IsChecked == true)
            query = query.Where(u => u.IsActive);
        if (ChkInactive.IsChecked == true)
            query = query.Where(u => !u.IsActive);
        if (ChkActiveTasks.IsChecked == true)
            query = query.Where(u => u.Tasks.Any(t => t.Status == CRM.Data.Task.TaskStatus.Assigned || t.Status == CRM.Data.Task.TaskStatus.InProgress));
        if (ChkCompletedTasks.IsChecked == true)
            query = query.Where(u => u.Tasks.Any(t => t.Status == CRM.Data.Task.TaskStatus.Completed));

        UsersList.ItemsSource = query.ToList();
    }

    private void FilterChanged(object sender, RoutedEventArgs e)
    {
        LoadUsers();
    }

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if (UsersList.SelectedItem is not User user) return;
        new DetailsWindow(user).ShowDialog();
    }

    private void UsersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (UsersList.SelectedItem is not User user) return;
        new DetailsWindow(user).ShowDialog();
    }
}