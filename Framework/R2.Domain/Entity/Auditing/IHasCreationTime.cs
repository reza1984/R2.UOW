
using System;

namespace R2.Domain.Entity.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }

    public interface ICreationAudited<TUser> : IHasCreationTime
        where TUser : class
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        TUser CreatorUser { get; set; }
    }
    
   
}
