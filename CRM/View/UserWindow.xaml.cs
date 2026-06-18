using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CRM.View;

namespace CRM;

public partial class UserWindow : Window
{
    public UserWindow()
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
}