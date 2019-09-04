using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Maps.Data
{
    /// <summary>
    /// Template class the represents one Table in the database and 
    /// has basic methods for data manipulation.
    /// </summary>
    /// <typeparam name="TEntity">Type of table entity</typeparam>
    public class Repository<TEntity> where TEntity : class
    {
        internal MapsDbContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(MapsDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Returns entitys that meet filter requirements.
        /// </summary>
        /// <param name="filter">Returned entities will meet these requirements.</param>
        /// <param name="orderBy">Order of the returned entities</param>
        /// <param name="includeProperties">Related entities to be included in the query result</param>
        /// <returns>List od entity entities.</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Returns entity from the database with the given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Found entity or null.</returns>
        public virtual TEntity GetByID(object id)
        {
            if (id != null)
            {
                return dbSet.Find(id);
            }
            return null;
        }

        /// <summary>
        /// Inserts new entities in corresponding dbSet
        /// </summary>
        /// <param name="entity">New entity to be added to the dbSet.</param>
        public virtual void Insert(TEntity entity)
        {
            if (entity != null)
            {
                dbSet.Add(entity);
            }
        }

        /// <summary>
        /// Inserts list of entities in the corresponding dbSet.
        /// </summary>
        /// <param name="entities">List of entities to be added to the dbSet.</param>
        public virtual void BulkInsert(IList<TEntity> entities)
        {
            if (entities != null && entities.Count != 0)
            {
                context.BulkInsert(entities);
            }
        }

        /// <summary>
        /// Deletes entity with the given id.
        /// </summary>
        /// <param name="id">Id of the entity to be deleted.</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        /// <summary>
        /// Deltes given entity.
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted.</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete != null)
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            }
        }

        /// <summary>
        /// Updates given entity in the database.
        /// </summary>
        /// <param name="entityToUpdate">entity to be updated.</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                dbSet.Attach(entityToUpdate);
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }
    }
}

