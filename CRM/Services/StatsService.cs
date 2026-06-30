using CRM.Data;

namespace CRM;

public static class StatsService
{
    public static Dictionary<CRM.Data.Task.TaskStatus, int> GetTaskCounts(int? userId = null, int? clientId = null, DateTime? from = null, DateTime? to = null)
    {
        using var db = new AppDbContext();
        var query = db.Tasks.AsQueryable();

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        if (clientId.HasValue)
            query = query.Where(t => t.ClientId == clientId.Value);

        var tasks = query.ToList();

        if (from.HasValue)
            tasks = tasks.Where(t =>
                (t.StartDate.HasValue && t.StartDate.Value >= from.Value) ||
                (t.AcceptanceDate.HasValue && t.AcceptanceDate.Value >= from.Value) ||
                (t.CompletionDate.HasValue && t.CompletionDate.Value >= from.Value)
            ).ToList();
        if (to.HasValue)
            tasks = tasks.Where(t =>
                (t.StartDate.HasValue && t.StartDate.Value <= to.Value) ||
                (t.AcceptanceDate.HasValue && t.AcceptanceDate.Value <= to.Value) ||
                (t.CompletionDate.HasValue && t.CompletionDate.Value <= to.Value)
            ).ToList();

        return new Dictionary<CRM.Data.Task.TaskStatus, int>
        {
            [CRM.Data.Task.TaskStatus.Pending] = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Pending),
            [CRM.Data.Task.TaskStatus.Available] = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Available),
            [CRM.Data.Task.TaskStatus.Assigned] = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Assigned),
            [CRM.Data.Task.TaskStatus.InProgress] = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.InProgress),
            [CRM.Data.Task.TaskStatus.Completed] = tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Completed),
        };
    }

    public static int GetUserCount()
    {
        using var db = new AppDbContext();
        return db.Users.Count();
    }

    public static int GetClientCount()
    {
        using var db = new AppDbContext();
        return db.Clients.Count();
    }

    public static int GetPendingStatusRequestsCount()
    {
        using var db = new AppDbContext();
        return db.TaskStatusRequests.Count(r => !r.IsProcessed);
    }

    public static int GetPendingProfileRequestsCount()
    {
        using var db = new AppDbContext();
        return db.ProfileChangeRequests.Count(r => !r.IsProcessed);
    }

    public static int GetPendingOrdersCount()
    {
        using var db = new AppDbContext();
        return db.Tasks.Count(t => t.Status == CRM.Data.Task.TaskStatus.Pending);
    }
}
