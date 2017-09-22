namespace R2.Domain.Entity
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
