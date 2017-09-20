using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebComment.Data;

namespace WebComment.API.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        SqlDbContext GetDbContext();
        GenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void Save();
    }
}
