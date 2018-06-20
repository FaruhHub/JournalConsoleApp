using Magazine.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Repo
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DIConsoleEntities context;
        internal IDbSet<TEntity> dbSet;

        public Repository(DIConsoleEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public bool Insert(TEntity entity)
        {
            try
            {
                dbSet.Add(entity);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return false;
            }
        }
        public bool InsertCollection(List<TEntity> entityCollection)
        {
            try
            {
                entityCollection.ForEach(e =>
                {
                    dbSet.Add(e);
                });
                context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", sb.ToString(), ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        

        public List<TEntity> GetData()
        {
            try
            {
                return dbSet.ToList<TEntity>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return default(List<TEntity>);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
