using System.Windows;
using CRM.Data;
using CRM.ViewModel.AdminWindow;

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
    
    private void UserPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserListPage());
    }
    
    private void ClientsPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ClientListPage());
    }
    
    private void TasksPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TaskListPage());
    }
    
    private void RequestsBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TaskRequestsPage());
    }

    private void StatsBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new StatsPage());
    }
}