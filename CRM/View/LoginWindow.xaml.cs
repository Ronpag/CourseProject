using System.Windows;

namespace CRM.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }
    
    private void LoginBtn(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        
        Window currentWindow = Window.GetWindow(this);
        currentWindow?.Close();
    }
}