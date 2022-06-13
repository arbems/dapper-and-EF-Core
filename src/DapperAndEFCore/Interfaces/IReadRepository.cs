namespace DapperAndEFCore.Interfaces;

public interface IReadRepository<TEntity> : IReadRepositoryBase<TEntity> where TEntity : class { }