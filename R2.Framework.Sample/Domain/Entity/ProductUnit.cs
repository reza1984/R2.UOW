using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R2.Domain.Entity;

namespace R2.Framework.Sample.Domain.Entity
{
    public class ProductUnit : TAggregate
    {
        public string Name { get; set; }
    }

    public class ProductUnitTypeConfiguration : IEntityTypeConfiguration<ProductUnit>
    {
        public void Configure(EntityTypeBuilder<ProductUnit> builder)
        {
        }
    }
}
