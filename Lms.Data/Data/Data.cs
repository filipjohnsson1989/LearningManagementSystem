using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data;

public class LmsDbContext : DbContext
{
    public LmsDbContext(DbContextOptions<LmsDbContext> options):base(options)
    {

    }
}
