namespace CRM.Data;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }

    public ICollection<Task>? Tasks { get; set; }

    public override string ToString()
    {
        return $"ID: {Id} | {UserName} ({Name}) | Active: {IsActive}";
    }
}
