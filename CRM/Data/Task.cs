namespace CRM.Data;

public class Task
{
    public int Id { get; set; }

    public string TaskName { get; set; }

    public string Description { get; set; }

    public int ClientId { get; set; }

    public int? WorkerId { get; set; }

    public TaskStatus Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? AcceptanceDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public override string ToString()
    {
        string info = $"ID: {Id} | {TaskName} | Status: {Status}";

        if (StartDate.HasValue)
            info += $" | Start: {StartDate.Value:d}";

        if (AcceptanceDate.HasValue)
            info += $" | Accepted: {AcceptanceDate.Value:d}";

        if (CompletionDate.HasValue)
            info += $" | Done: {CompletionDate.Value:d}";

        return info;
    }

    public enum TaskStatus
    {
        Pending,
        Available,
        Assigned,
        InProgress,
        Completed
    }
}