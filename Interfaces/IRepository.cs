using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(string id);
        Task<bool> Delete(string id);
        Task<bool> Add(T obj);
    }
}
