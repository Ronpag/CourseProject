using System.Windows;

namespace CRM.View;

public partial class UserProfileWindow : Window
{
    private readonly int _userId;

    public UserProfileWindow(int userId)
    {
        InitializeComponent();
        _userId = userId;

        var user = UserService.GetById(userId);

        if (user == null)
        {
            MessageBox.Show("User not found");
            Close();
            return;
        }

        UserNameText.Text = user.UserName;
        LoginText.Text = user.Name;
        EmailBox.Text = user.Email ?? "";
        PhoneBox.Text = user.Phone ?? "";
        PositionBox.Text = user.Position ?? "";
    }

    private void SubmitBtn(object sender, RoutedEventArgs e)
    {
        string email = EmailBox.Text.Trim();
        string phone = PhoneBox.Text.Trim();
        string position = PositionBox.Text.Trim();

        if (RequestService.CreateProfileChangeRequest(
                userId: _userId, clientId: null,
                email, phone, position, isForUser: true))
        {
            DialogResult = true;
            Close();
        }
    }
}
