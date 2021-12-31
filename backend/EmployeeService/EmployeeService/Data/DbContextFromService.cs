using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Data
{
    //This class is for documentation on how to access EmployeeDbContext without DI
    public static class DbContextFromService
    {
        public static void SetupContext(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                UseContext(scope.ServiceProvider.GetService<EmployeeDbContext>());
            }
        }
        private static void UseContext(EmployeeDbContext context)
        {
            //Nothing so fare
        }
    }
}
