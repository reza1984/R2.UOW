using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R2.Domain.Entity;
using R2.Domain.Entity.Auditing;

namespace R2.Framework.Sample.Domain.Entity
{
    public class ProductUnit : TAggregate,IHasCreationTime,IHasModificationTime,IHasDeletionTime
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
