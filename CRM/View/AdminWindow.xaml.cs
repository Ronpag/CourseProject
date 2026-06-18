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
    
    private void StartRegPage_click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new RegistrationPage());
    }
}