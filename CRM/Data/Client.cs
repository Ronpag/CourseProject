namespace CRM.Data;

public class Client
{
    public int Id { get; set; }
    public string NameClient { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int CountOrders { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    public ICollection<Task>? Tasks { get; set; }

    public override string ToString()
    {
        return $"ID: {Id} | Client: {NameClient} | Orders: {CountOrders}";
    }
}
