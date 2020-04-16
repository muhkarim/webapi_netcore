using JWT_API_NETCORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_API_NETCORE.Repository.Interface
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> Get();
        Task<T> Get(int Id);
        Task<T> Post(T entity);
        Task<T> Put(T entity);
        Task<T> Delete(int Id);
    }
}
