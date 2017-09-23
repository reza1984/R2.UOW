using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R2.Domain.Entity;

namespace R2.Framework.Sample.Domain.Entity
{
    public class ProductCategory : TAggregate
    {
        public ProductCategory()
        {
            SubCategories = new List<ProductCategory>();
        }
        public string Name { get; set; }
        public virtual ProductCategory ParentCategory { get; set; }
        public Guid? ParentId { get; set; }
        public virtual ICollection<ProductCategory> SubCategories { get; set; }
    }

    public class ProductCategoryTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasIndex(c=>c.ParentId);

            builder.HasOne(c=>c.ParentCategory)
                   .WithMany(c=>c.SubCategories)
                   .HasForeignKey(c=>c.ParentId);
        }
    }
}
