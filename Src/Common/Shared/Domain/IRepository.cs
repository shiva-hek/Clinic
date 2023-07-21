namespace Shared.Domain
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity? Get(Guid id);
    }
}
