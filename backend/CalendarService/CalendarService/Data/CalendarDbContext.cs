using CalendarService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Data
{
    public class CalendarDbContext : DbContext
    {
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<CalendarEmployeeModel> CalendarEmployee { get; set; }
        public DbSet<CalendarModel> Calendar { get; set; }
        public DbSet<CalendarTaskObjModel> CalendarTaskObj { get; set; }
    }
}
