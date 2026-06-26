namespace CRM.Data;

public class Client
{
    public int Id { get; set; }
    public string NameClient { get; set; }
    public int CountOrders { get; set; }

    public override string ToString()
    {
        return $"ID: {Id} | Client: {NameClient} | Orders: {CountOrders}";
    }
}