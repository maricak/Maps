﻿using Maps.Entities;
using System;

namespace Maps.Data
{
    public class DataAccess : IDisposable
    {
        private readonly MapsDbContext context = new MapsDbContext();

        public Repository<Map> Maps
        {
            get
            {
                return new Repository<Map>(context);
            }
        }
        public Repository<Layer> Layers
        {
            get
            {
                return new Repository<Layer>(context);
            }
        }
        public Repository<User> Users
        {
            get
            {
                return new Repository<User>(context);
            }
        }
        public Repository<Column> Columns
        {
            get
            {
                return new Repository<Column>(context);
            }
        }

        public Repository<Entities.Data> Data
        {
            get
            {
                return new Repository<Entities.Data>(context);
            }
        }

        public void Save()
        {
            context.BatchSaveChanges();
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
