using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetData();
        bool Insert(TEntity entity);
        bool InsertCollection(List<TEntity> entityCollection);

        void Dispose();
    }
}
