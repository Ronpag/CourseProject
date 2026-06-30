using System.Windows;
using CRM.Data;

namespace CRM;

public static class RequestService
{
    public static List<TaskStatusRequest> GetPendingStatusRequests()
    {
        using var db = new AppDbContext();
        return db.TaskStatusRequests.Where(r => !r.IsProcessed).ToList();
    }

    public static bool ApproveStatusRequest(int requestId)
    {
        using var db = new AppDbContext();

        var request = db.TaskStatusRequests.FirstOrDefault(r => r.Id == requestId);
        if (request == null) return false;

        var task = db.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
        if (task == null)
        {
            MessageBox.Show("Task not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (request.RequestedStatus == CRM.Data.Task.TaskStatus.Completed)
        {
            DateTime completionDate = request.RequestedCompletionDate ?? DateTime.Now;

            if (task.AcceptanceDate.HasValue && completionDate < task.AcceptanceDate.Value)
            {
                MessageBox.Show("Completion date cannot be earlier than acceptance date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            task.CompletionDate = completionDate;
        }

        task.Status = request.RequestedStatus;
        request.IsProcessed = true;
        request.IsApproved = true;

        db.SaveChanges();
        return true;
    }

    public static bool RejectStatusRequest(int requestId)
    {
        using var db = new AppDbContext();

        var request = db.TaskStatusRequests.FirstOrDefault(r => r.Id == requestId);
        if (request == null) return false;

        request.IsProcessed = true;
        request.IsApproved = false;
        db.SaveChanges();
        return true;
    }

    public static bool ApproveOrder(int taskId)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        task.Status = CRM.Data.Task.TaskStatus.Available;
        db.SaveChanges();
        return true;
    }

    public static bool DeleteOrder(int taskId)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        var client = db.Clients.FirstOrDefault(c => c.Id == task.ClientId);
        if (client != null)
            client.CountOrders--;

        db.Tasks.Remove(task);
        db.SaveChanges();
        return true;
    }

    public static List<ProfileChangeRequest> GetPendingProfileRequests()
    {
        using var db = new AppDbContext();
        return db.ProfileChangeRequests.Where(r => !r.IsProcessed).ToList();
    }

    public static bool CreateProfileChangeRequest(int? userId, int? clientId,
        string? email, string? phone, string? extraField, bool isForUser)
    {
        if (!string.IsNullOrWhiteSpace(email) && !ValidationService.ValidateEmail(email))
            return false;

        if (!string.IsNullOrWhiteSpace(phone) && !ValidationService.ValidatePhone(phone))
            return false;

        using var db = new AppDbContext();

        if (isForUser)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string? newEmail = email != (user.Email ?? "") ? email : null;
            string? newPhone = phone != (user.Phone ?? "") ? phone : null;
            string? newPosition = extraField != (user.Position ?? "") ? extraField : null;

            if (newEmail == null && newPhone == null && newPosition == null)
            {
                MessageBox.Show("No changes detected", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var existing = db.ProfileChangeRequests.FirstOrDefault(r => r.UserId == userId && !r.IsProcessed);
            if (existing != null)
            {
                existing.NewEmail = newEmail;
                existing.NewPhone = newPhone;
                existing.NewPosition = newPosition;
            }
            else
            {
                db.ProfileChangeRequests.Add(new ProfileChangeRequest
                {
                    UserId = userId,
                    NewEmail = newEmail,
                    NewPhone = newPhone,
                    NewPosition = newPosition,
                    IsProcessed = false
                });
            }
        }
        else
        {
            var client = db.Clients.FirstOrDefault(c => c.Id == clientId);
            if (client == null)
            {
                MessageBox.Show("Client not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string? newEmail = email != (client.Email ?? "") ? email : null;
            string? newPhone = phone != (client.Phone ?? "") ? phone : null;
            string? newAddress = extraField != (client.Address ?? "") ? extraField : null;

            if (newEmail == null && newPhone == null && newAddress == null)
            {
                MessageBox.Show("No changes detected", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var existing = db.ProfileChangeRequests.FirstOrDefault(r => r.ClientId == clientId && !r.IsProcessed);
            if (existing != null)
            {
                existing.NewEmail = newEmail;
                existing.NewPhone = newPhone;
                existing.NewAddress = newAddress;
            }
            else
            {
                db.ProfileChangeRequests.Add(new ProfileChangeRequest
                {
                    ClientId = clientId,
                    NewEmail = newEmail,
                    NewPhone = newPhone,
                    NewAddress = newAddress,
                    IsProcessed = false
                });
            }
        }

        db.SaveChanges();
        return true;
    }

    public static bool ApproveProfileRequest(int requestId)
    {
        using var db = new AppDbContext();

        var request = db.ProfileChangeRequests.FirstOrDefault(r => r.Id == requestId);
        if (request == null) return false;

        if (request.NewEmail != null && !ValidationService.ValidateEmail(request.NewEmail))
            return false;

        if (request.NewPhone != null && !ValidationService.ValidatePhone(request.NewPhone))
            return false;

        if (request.UserId != null)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (request.NewEmail != null) user.Email = request.NewEmail;
            if (request.NewPhone != null) user.Phone = request.NewPhone;
            if (request.NewPosition != null) user.Position = request.NewPosition;
        }
        else if (request.ClientId != null)
        {
            var client = db.Clients.FirstOrDefault(c => c.Id == request.ClientId);
            if (client == null)
            {
                MessageBox.Show("Client not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (request.NewEmail != null) client.Email = request.NewEmail;
            if (request.NewPhone != null) client.Phone = request.NewPhone;
            if (request.NewAddress != null) client.Address = request.NewAddress;
        }

        request.IsProcessed = true;
        request.IsApproved = true;
        db.SaveChanges();
        return true;
    }

    public static bool RejectProfileRequest(int requestId)
    {
        using var db = new AppDbContext();

        var request = db.ProfileChangeRequests.FirstOrDefault(r => r.Id == requestId);
        if (request == null) return false;

        request.IsProcessed = true;
        request.IsApproved = false;
        db.SaveChanges();
        return true;
    }
}
