using System.Windows;
using CRM.Data;

namespace CRM;

public static class UserService
{
    public static List<User> GetAll()
    {
        using var db = new AppDbContext();
        return db.Users.ToList();
    }

    public static List<User> GetUsers()
    {
        using var db = new AppDbContext();
        return db.Users.Where(u => !u.IsAdmin).ToList();
    }

    public static User? GetById(int id)
    {
        using var db = new AppDbContext();
        return db.Users.FirstOrDefault(u => u.Id == id);
    }

    public static bool LoginExists(string login, int? excludeId = null)
    {
        using var db = new AppDbContext();
        if (excludeId.HasValue)
            return db.Users.Any(u => u.Name == login && u.Id != excludeId.Value);
        return db.Users.Any(u => u.Name == login);
    }

    public static bool LoginExistsAnywhere(string login)
    {
        using var db = new AppDbContext();
        return db.Users.Any(u => u.Name == login) || db.Clients.Any(c => c.Login == login);
    }

    public static User? Authenticate(string login, string password)
    {
        using var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Name == login);
        if (user == null) return null;
        if (!PasswordService.Verify(password, user.Password)) return null;
        return user;
    }

    public static bool Create(string name, string UserName, string password, bool isAdmin, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            MessageBox.Show("User name cannot be empty", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!ValidationService.ValidateLoginPassword(name, password))
            return false;

        if (LoginExistsAnywhere(name))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        using var db = new AppDbContext();
        db.Users.Add(new User
        {
            Name = name,
            UserName = UserName,
            Password = PasswordService.Hash(password),
            IsAdmin = isAdmin,
            IsActive = isActive
        });
        db.SaveChanges();
        return true;
    }

    public static bool Update(int id, string name, string UserName, string? newPassword, bool? isActive)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(UserName))
        {
            MessageBox.Show("Login and name cannot be empty", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (LoginExists(name, id))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            if (!ValidationService.ValidateLoginPassword(name, newPassword))
                return false;
        }

        using var db = new AppDbContext();

        if (db.Clients.Any(c => c.Login == name))
        {
            MessageBox.Show("Login already exists", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        user.Name = name;
        user.UserName = UserName;

        if (!string.IsNullOrWhiteSpace(newPassword))
            user.Password = PasswordService.Hash(newPassword);

        if (isActive.HasValue)
            user.IsActive = isActive.Value;

        db.SaveChanges();
        return true;
    }

    public static bool Delete(int id)
    {
        using var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        if (user.IsAdmin)
        {
            MessageBox.Show("Cannot delete admin", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        db.Users.Remove(user);
        db.SaveChanges();
        return true;
    }
}