using System.Linq;
using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class UserProfileWindow : Window
{
    private readonly int _userId;

    public UserProfileWindow(int userId)
    {
        InitializeComponent();
        _userId = userId;

        using var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            MessageBox.Show("User not found");
            Close();
            return;
        }

        WorkerNameText.Text = user.WorkerName;
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

        using var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Id == _userId);

        if (user == null)
        {
            MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        bool hasChanges = false;

        if (email != (user.Email ?? "")) hasChanges = true;
        if (phone != (user.Phone ?? "")) hasChanges = true;
        if (position != (user.Position ?? "")) hasChanges = true;

        if (!hasChanges)
        {
            MessageBox.Show("No changes to submit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var request = new ProfileChangeRequest
        {
            UserId = _userId,
            NewEmail = email != (user.Email ?? "") ? email : null,
            NewPhone = phone != (user.Phone ?? "") ? phone : null,
            NewPosition = position != (user.Position ?? "") ? position : null,
            IsProcessed = false
        };

        db.ProfileChangeRequests.Add(request);
        db.SaveChanges();

        MessageBox.Show(
            "Profile change request submitted for admin approval.",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        DialogResult = true;
        Close();
    }
}
