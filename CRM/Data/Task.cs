namespace CRM.Data;

public class Task
{
    public int Id { get; set; }

    public string TaskName { get; set; }

    public int ClientId { get; set; }

    public int? WorkerId { get; set; }

    public TaskStatus Status { get; set; }

    public override string ToString()
    {
        return $"ID: {Id} | Task: {TaskName} | ClientId: {ClientId} | WorkerId: {(WorkerId.HasValue ? WorkerId : "Not assigned")} | Status: {Status}";
    }

    public enum TaskStatus
    {
        Available,
        Assigned,
        InProgress,
        Completed
    }
}