namespace CRM.Data;

public class TaskStatusRequest
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public Task.TaskStatus RequestedStatus { get; set; }

    public string Comment { get; set; }

    public bool IsProcessed { get; set; }

    public bool? IsApproved { get; set; }
    
    public override string ToString()
    {
        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == TaskId);

        string s = $"Task #{TaskId} ({task?.TaskName}) -> {RequestedStatus}";

        if (!string.IsNullOrWhiteSpace(Comment))
            s += $" | Comment: {Comment}";

        return s;
    }
}