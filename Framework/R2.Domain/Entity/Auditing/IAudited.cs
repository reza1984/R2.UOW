namespace R2.Domain.Entity.Auditing
{


    public interface IAudited<TUser> :  ICreationAudited<TUser>, IModificationAudited<TUser>
        where TUser : class
    {

    }
}
