using TaskService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Data
{
    public class TaskObjDbContext : DbContext
    {
        public TaskObjDbContext(DbContextOptions<TaskObjDbContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<TaskObject> TaskObjs { get; set; }
    }
}
