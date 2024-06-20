namespace shortener_back.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    
    Task<long> Count();

    Task<IEnumerable<TEntity>> GetRange(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate
    );

    Task<TEntity?> GetById(object id);
    
    Task<bool> Exists(object id);

    Task Insert(TEntity entity);

    Task Delete(object id);

    Task Delete(TEntity entity);

    Task Update(TEntity entity);

    Task Save();
}
