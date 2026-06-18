using System.Configuration;
using System.Data;
using System.Windows;
using CRM.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        using var db = new AppDbContext();

        db.Database.Migrate();

        base.OnStartup(e);
    }
}