using Maps.Entities;
using System;

namespace Maps.Data
{
    public class DataAccess : IDisposable
    {
        private readonly MapsDbContext context = new MapsDbContext();
        private Repository<Map> maps;
        private Repository<Layer> layers;

        public Repository<Map> Maps
        {
            get
            {
                if (maps == null)
                {
                    maps = new Repository<Map>(context);
                }
                return maps;
            }
        }

        public Repository<Layer> Layers
        {
            get
            {
                if (layers == null)
                {
                    layers = new Repository<Layer>(context);
                }
                return layers;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
