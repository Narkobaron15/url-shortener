namespace shortener_back.DataAccess.Repository;

public sealed class Repository<TEntity>(ShortenerDbContext context)
    : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task Save() => await context.SaveChangesAsync();

    public IQueryable<TEntity> GetAll() => _dbSet;

    public async Task<long> Count() => await _dbSet.LongCountAsync();

    public async Task<IEnumerable<TEntity>> GetRange(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate
    ) => await (predicate is null ? _dbSet : _dbSet.Where(predicate))
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    public async Task<TEntity?> GetById(object id)
        => await _dbSet.FindAsync(id);

    public async Task<bool> Exists(object id)
        => await GetById(id) is not null;

    public async Task Insert(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public async Task Delete(object id)
    {
        TEntity? entity = await _dbSet.FindAsync(id);
        if (entity is null) return;
        await Delete(entity);
    }

    public async Task Delete(TEntity entity)
        => await Task.Run(() =>
        {
            if (context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        });

    public async Task Update(TEntity entity)
        => await Task.Run(() =>
        {
            if (context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Update(entity);
        });
}
