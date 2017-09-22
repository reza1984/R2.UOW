using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using R2.Domain.Context;
using R2.Domain.Entity;
using R2.Domain.Helpers;

namespace R2.Domain.Context
{
    public interface IDataContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void Dispose();
    }

    public class DataContext : DbContext, IDataContext
    {
        private readonly IConfiguration configuration;
        public DataContext() : base() { }
        public DataContext(IConfiguration config)
        {
            configuration = config;
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // string connectionString = Microsoft
                //     .Extensions
                //     .Configuration
                //     .ConfigurationExtensions
                //     .GetConnectionString(this.configuration, "DefaultConnection");

                optionsBuilder.UseInMemoryDatabase("connectionString");
            }
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            System.AppDomain.CurrentDomain.GetAssemblies().ToList()
            .ForEach(assemply =>
            {
                assemply.GetTypes().Where(x => x.BaseType != null && x.BaseType == (typeof(TAggregate)))
                .ToList().ForEach(entity =>
                    {
                        modelBuilder.Entity(entity.GetType());
                    });
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}