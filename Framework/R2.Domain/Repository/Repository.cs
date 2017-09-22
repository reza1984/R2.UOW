using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Metadata;
using R2.Domain.Entity;
using R2.Domain.Entity.Auditing;
using R2.Domain.Helpers;
using R2.Domain.Context;

namespace R2.Domain.Repository
{
    public class Repository<TEntity> : DbSet<TAggregate>, IRepository<TEntity> where TEntity : TAggregate
    {
        private readonly DbContext _dbContext;
        public DbSet<TEntity> Entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Entity = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Changes the table name. This require the tables in the same database.
        /// </summary>
        /// <param name="table"></param>
        /// <remarks>
        /// This only been used for supporting multiple tables in the same model. This require the tables in the same database.
        /// </remarks>
        public void ChangeTable(string table)
        {
            if (_dbContext.Model.FindEntityType(typeof(TEntity)).Relational() is RelationalEntityTypeAnnotations relational)
            {
                relational.TableName = table;
            }
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> that contains elements that satisfy the condition specified by predicate.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null, bool disableTracking = true)
        {
            IQueryable<TEntity> set;
            if (disableTracking)
            {
                set = Entity.AsNoTracking();
            }
            else
            {
                set = Entity;
            }

            if (predicate != null)
            {
                set = set.Where(predicate);
            }

            return set;
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = Entity;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = Entity;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
        }

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public IQueryable<TEntity> FromSql(string sql, params object[] parameters) => Entity.FromSql(sql, parameters);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The found entity or null.</returns>
        public TEntity Find(params object[] keyValues) => Entity.Find(keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A <see cref="Task{TEntity}" /> that represents the asynchronous insert operation.</returns>
        public Task<TEntity> FindAsync(params object[] keyValues) => Entity.FindAsync(keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{TEntity}"/> that represents the asynchronous find operation. The task result contains the found entity or null.</returns>
        public Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken) => Entity.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// Inserts a new entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public void Insert(TEntity entity)
        {
            if (entity is IHasCreationTime)
                entity.As<IHasCreationTime>().CreationTime = DateTime.Now;

            var entry = Entity.Add(entity);
        }

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public void Insert(params TEntity[] entities)
        {
            if (entities[0] is IHasCreationTime)
                entities.ToList().ForEach(item => item.As<IHasCreationTime>().CreationTime = DateTime.Now);
            Entity.AddRange(entities);
        }

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public void Insert(IEnumerable<TEntity> entities)
        {
            if (entities.FirstOrDefault() is IHasCreationTime)
                entities.ToList().ForEach(item => item.As<IHasCreationTime>().CreationTime = DateTime.Now);
            Entity.AddRange(entities);
        }

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity is IHasCreationTime)
                entity.As<IHasCreationTime>().CreationTime = DateTime.Now;
            return Entity.AddAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous insert operation.</returns>
        public Task InsertAsync(params TEntity[] entities)
        {
            if (entities.FirstOrDefault() is IHasCreationTime)
                entities.ToList().ForEach(item => item.As<IHasCreationTime>().CreationTime = DateTime.Now);

            return Entity.AddRangeAsync(entities);
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entities.FirstOrDefault() is IHasCreationTime)
                entities.ToList().ForEach(item => item.As<IHasCreationTime>().CreationTime = DateTime.Now);

            return Entity.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(TEntity entity)
        {
            if (entity is IHasModificationTime)
                entity.As<IHasModificationTime>().LastModificationTime = DateTime.Now;

            Entity.Update(entity);
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Update(params TEntity[] entities)
        {
            var entity = entities[0];
            if (entity is IHasModificationTime)
                foreach (var item in entities)
                {
                    item.As<IHasModificationTime>().LastModificationTime = DateTime.Now;
                }

            Entity.UpdateRange(entities);
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Update(IEnumerable<TEntity> entities)
        {
            var entity = entities.FirstOrDefault();
            if (entity is IHasModificationTime)
                foreach (var item in entities)
                {
                    item.As<IHasModificationTime>().LastModificationTime = DateTime.Now;
                }

            Entity.UpdateRange(entities);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(TEntity entity)
        {

            if (entity is ISoftDelete)
            {
                entity.As<ISoftDelete>().IsDeleted = true;

                if (entity is IHasDeletionTime)
                    entity.As<IHasDeletionTime>().DeletionTime = DateTime.Now;

                Update(entity);
            }
            else
            {
                // using a stub entity to mark for deletion
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public void Delete(object id)
        {
            var entity = Entity.Find(id);

            if (entity is ISoftDelete)
            {
                entity.As<ISoftDelete>().IsDeleted = true;

                if (entity is IHasDeletionTime)
                    entity.As<IHasDeletionTime>().DeletionTime = DateTime.Now;

                Update(entity);
            }
            else
            {
                // using a stub entity to mark for deletion
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Delete(params TEntity[] entities)
        {
            var entity = entities[0];

            if (entity is ISoftDelete)
            {
                entities.ToList().ForEach(item =>
                {
                    item.As<ISoftDelete>().IsDeleted = true;

                    if (item is IHasDeletionTime)
                        item.As<IHasDeletionTime>().DeletionTime = DateTime.Now;
                });

                Entity.UpdateRange(entities);
            }
            else
            {
                Entity.RemoveRange(entities);
            }
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Delete(IEnumerable<TEntity> entities)
        {
            var entity = entities.FirstOrDefault();

            if (entity is ISoftDelete)
            {
                entities.ToList().ForEach(item =>
                {
                    item.As<ISoftDelete>().IsDeleted = true;

                    if (item is IHasDeletionTime)
                        item.As<IHasDeletionTime>().DeletionTime = DateTime.Now;
                });

                Entity.UpdateRange(entities);
            }
            else
            {
                Entity.RemoveRange(entities);
            }
        }









    }
}
