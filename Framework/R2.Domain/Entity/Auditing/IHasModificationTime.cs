
using System;

namespace R2.Domain.Entity.Auditing
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }

    public interface IModificationAudited<TUser> : IHasModificationTime
       where TUser : class
    {
        TUser LastModifierUser { get; set; }
    }
}
