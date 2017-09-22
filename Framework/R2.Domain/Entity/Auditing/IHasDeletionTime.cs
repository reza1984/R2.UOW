
using System;

namespace R2.Domain.Entity.Auditing
{
    public interface IHasDeletionTime : ISoftDelete
    {
        DateTime? DeletionTime { get; set; }
    }

    public interface IDeletionAudited<TUser> : IHasDeletionTime
       where TUser : class
    {
        TUser DeleterUser { get; set; }
    }
}
