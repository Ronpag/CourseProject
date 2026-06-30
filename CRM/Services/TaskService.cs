using System.Windows;
using CRM.Data;

namespace CRM;

public static class TaskService
{
    public static List<CRM.Data.Task> GetFiltered(int? UserId = null, int? clientId = null,
        List<CRM.Data.Task.TaskStatus>? statusFilters = null,
        DateTime? dateFrom = null, DateTime? dateTo = null)
    {
        using var db = new AppDbContext();

        var query = db.Tasks.AsQueryable();

        if (UserId.HasValue)
            query = query.Where(t => t.UserId == UserId.Value);

        if (clientId.HasValue)
            query = query.Where(t => t.ClientId == clientId.Value);

        if (statusFilters != null && statusFilters.Count > 0)
            query = query.Where(t => statusFilters.Contains(t.Status));

        if (dateFrom.HasValue)
            query = query.Where(t => t.StartDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(t => t.StartDate <= dateTo.Value);

        return query.ToList();
    }

    public static List<CRM.Data.Task> GetAvailable()
    {
        using var db = new AppDbContext();
        return db.Tasks.Where(t => t.Status == CRM.Data.Task.TaskStatus.Available).ToList();
    }

    public static CRM.Data.Task? GetById(int id)
    {
        using var db = new AppDbContext();
        return db.Tasks.FirstOrDefault(t => t.Id == id);
    }

    public static bool Create(string taskName, string? description, int clientId, int? UserId,
        CRM.Data.Task.TaskStatus status, DateTime? startDate, DateTime? acceptanceDate)
    {
        if (string.IsNullOrWhiteSpace(taskName))
        {
            MessageBox.Show("Task name cannot be empty", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        using var db = new AppDbContext();

        var client = db.Clients.FirstOrDefault(c => c.Id == clientId);
        if (client == null)
        {
            MessageBox.Show("Client not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var task = new CRM.Data.Task
        {
            TaskName = taskName,
            Description = description ?? "",
            ClientId = clientId,
            UserId = UserId,
            Status = status,
            StartDate = startDate,
            AcceptanceDate = acceptanceDate
        };

        db.Tasks.Add(task);
        client.CountOrders++;
        db.SaveChanges();
        return true;
    }

    public static bool Update(int taskId, string taskName, string? description, int clientId, int? UserId,
        CRM.Data.Task.TaskStatus status, DateTime? startDate, DateTime? acceptanceDate, DateTime? completionDate)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        var oldClientId = task.ClientId;

        task.TaskName = taskName;
        if (description != null) task.Description = description;
        task.Status = status;
        task.StartDate = startDate;
        task.AcceptanceDate = acceptanceDate;
        task.CompletionDate = completionDate;

        if (UserId.HasValue)
            task.UserId = UserId;

        if (clientId != oldClientId)
        {
            var oldClient = db.Clients.FirstOrDefault(c => c.Id == oldClientId);
            if (oldClient != null && oldClient.CountOrders > 0)
                oldClient.CountOrders--;

            var newClient = db.Clients.FirstOrDefault(c => c.Id == clientId);
            if (newClient != null)
                newClient.CountOrders++;

            task.ClientId = clientId;
        }

        db.SaveChanges();
        return true;
    }

    public static bool Delete(int taskId)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        var client = db.Clients.FirstOrDefault(c => c.Id == task.ClientId);
        if (client != null && client.CountOrders > 0)
            client.CountOrders--;

        db.Tasks.Remove(task);
        db.SaveChanges();
        return true;
    }

    public static bool TakeTask(int taskId, int UserId)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        if (task.StartDate.HasValue && DateTime.Now < task.StartDate.Value)
        {
            MessageBox.Show("Task cannot be accepted before its start date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        task.UserId = UserId;
        task.Status = CRM.Data.Task.TaskStatus.Assigned;
        task.AcceptanceDate = DateTime.Now;
        db.SaveChanges();
        return true;
    }

    public static bool ChangeStatus(int taskId, CRM.Data.Task.TaskStatus newStatus, string? comment, DateTime? completionDate)
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        var request = new TaskStatusRequest
        {
            TaskId = taskId,
            RequestedStatus = newStatus,
            Comment = comment ?? "",
            RequestedCompletionDate = completionDate,
            IsProcessed = false
        };

        db.TaskStatusRequests.Add(request);
        db.SaveChanges();
        return true;
    }
}
