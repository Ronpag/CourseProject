namespace CRM.Data;

public class ProfileChangeRequest
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? ClientId { get; set; }
    public string? NewEmail { get; set; }
    public string? NewPhone { get; set; }
    public string? NewPosition { get; set; }
    public string? NewAddress { get; set; }
    public bool IsProcessed { get; set; }
    public bool? IsApproved { get; set; }

    public override string ToString()
    {
        using var db = new AppDbContext();

        string who;
        if (UserId != null)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == UserId);
            who = $"User: {user?.UserName ?? "N/A"} (ID: {UserId})";
        }
        else if (ClientId != null)
        {
            var client = db.Clients.FirstOrDefault(c => c.Id == ClientId);
            who = $"Client: {client?.NameClient ?? "N/A"} (ID: {ClientId})";
        }
        else
        {
            who = "Unknown";
        }

        string changes = "";
        if (NewEmail != null) changes += $" Email:{NewEmail}";
        if (NewPhone != null) changes += $" Phone:{NewPhone}";
        if (NewPosition != null) changes += $" Position:{NewPosition}";
        if (NewAddress != null) changes += $" Address:{NewAddress}";

        return $"{who} | Changes:{changes}";
    }
}
