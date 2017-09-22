// using System.Linq;
// using System.Reflection;
// using Microsoft.EntityFrameworkCore;
// using R2.Domain.Context;
// using R2.Framework.Sample.Domain.Entity;

// namespace R2.Framework.Sample.Domain.Context
// {
//     public class DataContext : DbContext
//     {
//         public DataContext(DbContextOptions<DataContext> options) : base(options)
//         {

//         }

//         public DataContext()
//         {
//         }

//         // public DbSet<ProductUnit> ProductUnits { get; set; }
//         public new DbSet<TEntity> Set<TEntity>() where TEntity : class
//         {
//             return base.Set<TEntity>();
//         }



//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {

//             //   var typesToRegister = from property in typeof(DataContext).GetProperties(BindingFlags.Public | BindingFlags.Instance)
//             //                       where
//             //                           ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
//             //                           ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(TEntity))
//             //                       select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);


//             modelBuilder.Entity<ProductUnit>();
//             modelBuilder.Entity<ProductCategory>();
//             modelBuilder.Entity<Product>();
//             base.OnModelCreating(modelBuilder);
//         }

//     }


// }
