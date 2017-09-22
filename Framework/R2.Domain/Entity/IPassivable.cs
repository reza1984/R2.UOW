namespace R2.Domain.Entity
{
    public interface IPassivable
    {
        bool IsActive { get; set; }
    }
}
