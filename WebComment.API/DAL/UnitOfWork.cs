using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using WebComment.Data;

namespace WebComment.API.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlDbContext context;
        private Dictionary<string, dynamic> _repositories;

        public UnitOfWork(SqlDbContext context)
        {
            if (context == null || this.disposed)
            {
                context = new SqlDbContext();
                this._repositories = null;
            }
            this.context = context;
        }
      
        public SqlDbContext GetDbContext()
        {
            return new SqlDbContext();
        }

        public GenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<GenericRepository<TEntity>>();
            }

            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (GenericRepository<TEntity>)_repositories[type];
            }

            _repositories.Add(type, new GenericRepository<TEntity>(context));


            return _repositories[type];
        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        public bool IsDisposed { get { return this.disposed; } }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
