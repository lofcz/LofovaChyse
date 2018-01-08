using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    interface IDaoBase<T>
    {
        IList<T> GetAll();
        object Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetbyId(int id);

    }
}
