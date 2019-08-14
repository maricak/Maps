using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Maps.Data
{

    public class Repository<TEntity> where TEntity : class
    {
        internal MapsDbContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(MapsDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

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

        public virtual TEntity GetByID(object id)
        {
            if (id != null)
            {
                return dbSet.Find(id);
            }
            return null;
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity != null)
            {
                dbSet.Add(entity);
            }
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

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

