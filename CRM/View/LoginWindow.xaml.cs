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
        string login = string.IsNullOrEmpty(Login.Text) ? "": Login.Text;
        string password = string.IsNullOrEmpty(Password.Text) ? "": Password.Text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Empty login or password");
            return;
        }

        if (login == "admin" && password == "admin")
        {
            Console.WriteLine("Login admin success");
        
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        
            Window currentWindow = Window.GetWindow(this);
            currentWindow?.Close();
            return;
        }else if (login == "user" && password == "user")
        {
            Console.WriteLine("Login user success");
        
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        
            Window currentWindow = Window.GetWindow(this);
            currentWindow?.Close();
            return;
        }
        else
        {
            Console.WriteLine("Invalid login or password");
            return;
        }
    }
}