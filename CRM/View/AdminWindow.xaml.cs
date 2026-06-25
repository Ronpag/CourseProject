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
    
    private void WorkerPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new WorkerListPage());
    }
    
    private void ClientsPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ClientListPage());
    }
    
    private void TasksPageBtn(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TasksListPage());
    }
}