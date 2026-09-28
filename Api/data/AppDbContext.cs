using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.model;
using Microsoft.EntityFrameworkCore;

namespace Api.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    }
}