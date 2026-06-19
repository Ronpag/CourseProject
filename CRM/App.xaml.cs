using System.Configuration;
using System.Data;
using System.Windows;
using CRM.Data;
using CRM.View;
using Microsoft.EntityFrameworkCore;

namespace CRM;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //uncomment this after test
        /*using var db = new AppDbContext();

        db.Database.EnsureCreated();

        bool adminExists = db.Users.Any(u => u.IsAdmin);

        Window window;

        if (!adminExists)
        {
            window = new FSWindow();
        }
        else
        {
            window = new LoginWindow();
        }

        window.Show();*/
    }
}