using System.Linq;
using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class DetailsWindow : Window
{
    public DetailsWindow(TaskStatusRequest request)
    {
        InitializeComponent();

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == request.TaskId);

        Title = $"Status Request #{request.Id}";

        var text = $"""
            Request ID: {request.Id}
            Task ID: {request.TaskId}
            Task Name: {task?.TaskName ?? "N/A"}
            Client: {(task != null ? db.Clients.FirstOrDefault(c => c.Id == task.ClientId)?.NameClient : "N/A")}
            Worker: {(task?.WorkerId != null ? db.Users.FirstOrDefault(u => u.Id == task.WorkerId)?.WorkerName : "Not assigned")}

            Current Status: {task?.Status}
            Requested Status: {request.RequestedStatus}
            Comment: {request.Comment ?? "(no comment)"}

            Processed: {(request.IsProcessed ? "Yes" : "No")}
            Approved: {(request.IsApproved.HasValue ? (request.IsApproved.Value ? "Yes" : "No") : "N/A")}
            """;

        DetailsContent.Text = text;
    }

    public DetailsWindow(CRM.Data.Task task)
    {
        InitializeComponent();

        using var db = new AppDbContext();

        Title = $"Task #{task.Id} Details";

        var text = $"""
            Task ID: {task.Id}
            Task Name: {task.TaskName}
            Description: {task.Description ?? "(no description)"}

            Client: {db.Clients.FirstOrDefault(c => c.Id == task.ClientId)?.NameClient ?? "N/A"} (ID: {task.ClientId})
            Worker: {task.Worker?.WorkerName ?? (task.WorkerId != null ? db.Users.FirstOrDefault(u => u.Id == task.WorkerId)?.WorkerName : "Not assigned")}

            Status: {task.Status}

            Start Date: {task.StartDate?.ToString("yyyy-MM-dd") ?? "N/A"}
            Acceptance Date: {task.AcceptanceDate?.ToString("yyyy-MM-dd") ?? "N/A"}
            Completion Date: {task.CompletionDate?.ToString("yyyy-MM-dd") ?? "N/A"}
            """;

        DetailsContent.Text = text;
    }

    public DetailsWindow(User user)
    {
        InitializeComponent();

        Title = $"Worker #{user.Id} Details";

        var text = $"""
            ID: {user.Id}
            Worker Name: {user.WorkerName}
            Login: {user.Name}
            Email: {user.Email ?? "N/A"}
            Phone: {user.Phone ?? "N/A"}
            Position: {user.Position ?? "N/A"}

            Admin: {(user.IsAdmin ? "Yes" : "No")}
            Active: {(user.IsActive ? "Yes" : "No")}
            """;

        DetailsContent.Text = text;
    }

    public DetailsWindow(ProfileChangeRequest request)
    {
        InitializeComponent();

        using var db = new AppDbContext();

        string who;
        string details = "";

        if (request.UserId != null)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == request.UserId);
            who = $"Worker: {user?.WorkerName ?? "N/A"} (ID: {request.UserId})";
            details += $"\nCurrent Values:\n  Email: {user?.Email ?? "N/A"}\n  Phone: {user?.Phone ?? "N/A"}\n  Position: {user?.Position ?? "N/A"}";
        }
        else if (request.ClientId != null)
        {
            var client = db.Clients.FirstOrDefault(c => c.Id == request.ClientId);
            who = $"Client: {client?.NameClient ?? "N/A"} (ID: {request.ClientId})";
            details += $"\nCurrent Values:\n  Email: {client?.Email ?? "N/A"}\n  Phone: {client?.Phone ?? "N/A"}\n  Address: {client?.Address ?? "N/A"}";
        }
        else
        {
            who = "Unknown";
        }

        Title = $"Profile Change Request #{request.Id}";

        string newVals = "";
        if (request.NewEmail != null) newVals += $"\n  Email: {request.NewEmail}";
        if (request.NewPhone != null) newVals += $"\n  Phone: {request.NewPhone}";
        if (request.NewPosition != null) newVals += $"\n  Position: {request.NewPosition}";
        if (request.NewAddress != null) newVals += $"\n  Address: {request.NewAddress}";

        var text = $"""
            Request ID: {request.Id}
            {who}

            Requested Changes:{newVals}
            {details}
            """;

        DetailsContent.Text = text;
    }

    public DetailsWindow(Client client)
    {
        InitializeComponent();

        Title = $"Client #{client.Id} Details";

        var text = $"""
            ID: {client.Id}
            Name: {client.NameClient}
            Login: {client.Login}
            Email: {client.Email ?? "N/A"}
            Phone: {client.Phone ?? "N/A"}
            Address: {client.Address ?? "N/A"}
            Orders Count: {client.CountOrders}
            """;

        DetailsContent.Text = text;
    }
}
