using System.Configuration;
using System.Data;
using System.Windows;
using CRM.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var db = new AppDbContext();

        db.Database.EnsureCreated();
    }
}