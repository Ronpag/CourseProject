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
        string login = Login.Text?.Trim() ?? "";
        string password = Password.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Empty login or password", "Error");
            return;
        }

        if (!ValidationService.ValidateEnglishText(login, "Login") || !ValidationService.ValidateEnglishText(password, "Password"))
            return;

        var user = UserService.Authenticate(login, password);

        if (user != null)
        {
            if (!user.IsActive)
            {
                MessageBox.Show(
                    "Your account is disabled. Please contact administrator.",
                    "Access denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            Window window = user.IsAdmin
                ? new AdminWindow()
                : new UserWindow(user.Id);

            window.Show();
            Close();
            return;
        }

        var client = ClientService.Authenticate(login, password);

        if (client == null)
        {
            MessageBox.Show("Invalid login or password", "Error");
            return;
        }

        var clientWindow = new ClientWindow(client.Id);
        clientWindow.Show();
        Close();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        var window = new ClientRegistrationWindow();

        if (window.ShowDialog() == true)
        {
            MessageBox.Show(
                "Registration successful! You can now log in.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}