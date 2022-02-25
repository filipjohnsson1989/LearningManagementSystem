using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data;

public class LmsDbContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }

    public LmsDbContext(DbContextOptions<LmsDbContext> options):base(options)
    {

    }
}
