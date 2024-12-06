using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        void Create(T entity);
        void Delete(string id);
        List<T> GetAll();
        T GetByID(int id);
        void Update(string id, T entity);
    }
}
