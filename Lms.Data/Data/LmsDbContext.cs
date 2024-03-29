﻿using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data;

public class LmsDbContext : DbContext
{
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;

    public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options)
    {

    }
}
