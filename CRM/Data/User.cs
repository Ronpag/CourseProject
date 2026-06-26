namespace CRM.Data;

public class User
{
    public int Id { get; set; }
    public string Name {get; set;}
    public string Password {get; set;}
    public bool IsAdmin {get; set;}
    public bool IsActive {get; set;}
    
    public override string ToString()
    {
        return $"ID: {Id} | Login: {Name} | Active: {IsActive}";
    }
}