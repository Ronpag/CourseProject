using System.Windows;
using CRM.Data;

namespace CRM;

public static class ClientService
{
    public static List<Client> GetAll()
    {
        using var db = new AppDbContext();
        return db.Clients.ToList();
    }

    public static Client? GetById(int id)
    {
        using var db = new AppDbContext();
        return db.Clients.FirstOrDefault(c => c.Id == id);
    }

    public static bool LoginExists(string login, int? excludeId = null)
    {
        using var db = new AppDbContext();
        if (excludeId.HasValue)
            return db.Clients.Any(c => c.Login == login && c.Id != excludeId.Value);
        return db.Clients.Any(c => c.Login == login);
    }

    public static bool NameExists(string name, int? excludeId = null)
    {
        using var db = new AppDbContext();
        if (excludeId.HasValue)
            return db.Clients.Any(c => c.NameClient == name && c.Id != excludeId.Value);
        return db.Clients.Any(c => c.NameClient == name);
    }

    public static Client? Authenticate(string login, string password)
    {
        using var db = new AppDbContext();
        var client = db.Clients.FirstOrDefault(c => c.Login == login);
        if (client == null) return null;
        if (!PasswordService.Verify(password, client.Password)) return null;
        return client;
    }

    public static bool Create(string name, string login, string password)
    {
        if (!ValidationService.ValidateEnglishText(name, "Client name"))
            return false;

        if (!ValidationService.ValidateLoginPassword(login, password))
            return false;

        if (NameExists(name))
        {
            MessageBox.Show("Client name already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (LoginExists(login))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        using var db = new AppDbContext();
        if (db.Users.Any(u => u.Name == login))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        db.Clients.Add(new Client
        {
            NameClient = name,
            Login = login,
            Password = PasswordService.Hash(password),
            CountOrders = 0
        });
        db.SaveChanges();
        return true;
    }

    public static bool Update(int id, string name, string login, string? newPassword, int countOrders)
    {
        if (!ValidationService.ValidateEnglishText(name, "Client name"))
            return false;

        if (NameExists(name, id))
        {
            MessageBox.Show("Client name already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (LoginExists(login, id))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        using var db = new AppDbContext();
        if (db.Users.Any(u => u.Name == login))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        var client = db.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null) return false;

        client.NameClient = name;
        client.Login = login;
        client.CountOrders = countOrders;

        if (!string.IsNullOrWhiteSpace(newPassword))
            client.Password = PasswordService.Hash(newPassword);

        db.SaveChanges();
        return true;
    }

    public static bool Delete(int id)
    {
        using var db = new AppDbContext();
        var client = db.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null) return false;

        db.Clients.Remove(client);
        db.SaveChanges();
        return true;
    }
}