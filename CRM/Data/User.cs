namespace CRM.Data;

public class User
{
    public int Id { get; set; }
    public string Name {get; set;}
    public string Password {get; set;}
    public string WorkerName {get; set;}
    public bool IsAdmin {get; set;}
    public bool IsActive {get; set;}
    
    public override string ToString()
    {
        return $"ID: {Id} | {WorkerName} ({Name}) | Active: {IsActive}";
    }
}