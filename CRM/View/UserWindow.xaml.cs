using System.Windows;
using CRM.View;
using CRM.ViewModel.UserWindow;

namespace CRM;

public partial class UserWindow : Window
{
    private readonly int _userId;

    public UserWindow(int userId)
    {
        InitializeComponent();
        _userId = userId;
    }

    private void BackBtn(object sender, RoutedEventArgs e)
    {
        LoginWindow login = new LoginWindow();
        login.Show();

        Close();
    }

    private void UserPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserPage(_userId));
    }
    
    private void AvailableBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AvailableTasksPage(_userId));
    }
}