using DrawManager.Domain;
using DrawManager.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DrawManager.Database.SqlServer
{
    public class DrawManagerSqlServerDbContextFactory : IDesignTimeDbContextFactory<DrawManagerSqlServerDbContext>
    {
        public DrawManagerSqlServerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DrawManagerSqlServerDbContext>();
            var configuration = ApplicationConfiguration.GetConfiguration(Directory.GetCurrentDirectory());

            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine(configuration);

            builder.UseSqlServer(configuration.GetConnectionString(Constants.CONNECTION_STRING_NAME_SQL_SERVER));

            return new DrawManagerSqlServerDbContext(builder.Options);
        }
    }
}
