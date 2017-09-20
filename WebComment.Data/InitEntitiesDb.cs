using System;
using System.Data.Entity.Infrastructure;
using System.Transactions;
using WebComment.Data;

namespace WebComment.Data
{
    public class InitEntitiesDb
    {
        public void InitializeDatabase(SqlDbContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                // create all tables
                var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                context.Database.ExecuteSqlCommand(dbCreationScript);

                //SeedEntities(context);
            }
            else
            {
                throw new ApplicationException("No database instance");
            }
        }

        public void InitMainDatabase()
        {
            var dbContext = new SqlDbContext();
            if (!dbContext.Database.Exists())
            {
                dbContext.Database.Create();
                //SeedEntities(dbContext);
            }
        }
    }
}
