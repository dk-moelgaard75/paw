using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeregService.Models;

namespace TimeregService.Data
{
    public class TimeRegDbContext : DbContext
    {
        public TimeRegDbContext(DbContextOptions<TimeRegDbContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<TimeRegistration> TimeReg { get; set; }
    }
}
