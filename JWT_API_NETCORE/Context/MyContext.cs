using JWT_API_NETCORE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_API_NETCORE.Context
{
    public class MyContext : IdentityDbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
    }
}
