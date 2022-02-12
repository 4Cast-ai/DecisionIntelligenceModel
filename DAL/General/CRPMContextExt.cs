using Infrastructure.Core;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class Context : DbContext
    {
        //static LoggerFactory object
        public static readonly ILoggerFactory FourCastLoggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
                //.AddProvider(new ColoredConsoleLoggerProvider(new ColoredConsoleLoggerConfiguration()));
            });

        /// <summary> total count of all savechanges within single transaction /// </summary>
        private int _changesCounter;
        public int GetChangesCounter() => _changesCounter;
        public void ClearChangesCounter() => _changesCounter = 0;

        public override int SaveChanges()
        {
            return _changesCounter += base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _changesCounter += await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(GeneralContext.GetConnectionString("4CASTDatabase"));
            }
        }
    }
}
