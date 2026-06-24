namespace CRM.Data;

public class Task
{
    public int Id { get; set; }
    public string NameTask {get; set;}
    public int ClientId {get; set;}
    public int WorkerId {get; set;}
    
    public override string ToString()
    {
        return $"ID: {Id} | Task: {NameTask} | ClientId: {ClientId} | WorkerId: {WorkerId}";
    }
}